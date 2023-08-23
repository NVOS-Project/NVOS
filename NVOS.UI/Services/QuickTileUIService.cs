using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.UI.Models;
using NVOS.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Management;

namespace NVOS.UI.Services
{
    [ServiceType(ServiceType.Singleton)]
    [ServiceDependency(typeof(UpdateProviderService))]
    public class QuickTileUIService : IService, IDisposable
    {
        private UpdateProviderService updateProvider;
        private bool isDisposed;

        private GameObject rightWristAnchor;
        private GameObject leftWristAnchor;

        private Window3D tileWindow;
        private GridLayoutPanel tileGrid;

        private List<Control> tiles;

        public void Init()
        {
            updateProvider = ServiceLocator.Resolve<UpdateProviderService>();

            leftWristAnchor = new GameObject("Left Wrist");
            rightWristAnchor = new GameObject("Right Wrist");

            tiles = new List<Control>();

            WristWindowSetup();

            GameObject leftGazeObject = new GameObject("Gaze Interactor");
            Transform leftGazeTransform = leftGazeObject.transform;
            leftGazeTransform.SetParent(leftWristAnchor.transform, false);
            leftGazeTransform.position = new Vector3(0f, 0f, 0.05f);
            leftGazeTransform.Rotate(new Vector3(90f, 0f, 0f));

            XRGazeInteractor leftGazeInteractor = leftGazeObject.AddComponent<XRGazeInteractor>();
            leftGazeInteractor.enableUIInteraction = false;

            GameObject rightGazeObject = new GameObject("Gaze Interactor");
            Transform rightGazeTransform = rightGazeObject.transform;
            rightGazeTransform.SetParent(rightWristAnchor.transform, false);
            rightGazeTransform.position = new Vector3(0f, 0f, 0.05f);
            rightGazeTransform.Rotate(new Vector3(90f, 0f, 0f));

            XRGazeInteractor rightGazeInteractor = rightGazeObject.AddComponent<XRGazeInteractor>();
            rightGazeInteractor.enableUIInteraction = false;

            GameObject interactableObject = new GameObject("Gaze Interactable");
            interactableObject.transform.SetParent(Camera.main.transform);
            interactableObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            interactableObject.AddComponent<SphereCollider>().radius = 0.5f;

            XRSimpleInteractable gazeInteractable = interactableObject.AddComponent<XRSimpleInteractable>();
            gazeInteractable.allowGazeInteraction = true;

            gazeInteractable.hoverEntered.AddListener(new UnityAction<HoverEnterEventArgs>(OnHoverEntered));
            gazeInteractable.hoverExited.AddListener(new UnityAction<HoverExitEventArgs>(OnHoverExited));

            XRHandSubsystem handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
            handSubsystem.updatedHands += OnHandUpdate;

            updateProvider.OnLateUpdate += UpdateProvider_OnLateUpdate;
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            updateProvider.OnLateUpdate -= UpdateProvider_OnLateUpdate;
            updateProvider = null;

            GameObject.Destroy(leftWristAnchor);
            leftWristAnchor = null;
            GameObject.Destroy(rightWristAnchor);
            rightWristAnchor = null;
            GameObject.Destroy(tileWindow.GetRootObject());
            tileWindow = null;

            foreach (Control tile in tiles)
            {
                tile.Dispose();
            }

            tiles = null;
            isDisposed = true;
        }

        private void WristWindowSetup()
        {
            tileWindow = new Window3D("Quick Tiles", 18f, 20f);
            tileWindow.GetContent().BackgroundColor = Color.gray;

            tileGrid = new GridLayoutPanel("Tile Grid");
            tileGrid.CellSize = new Vector2(5f, 5f);
            tileGrid.Spacing = new Vector2(0.5f, 0.5f);
            tileGrid.PaddingLeft = 1;
            tileGrid.PaddingRight = 1;
            tileGrid.PaddingTop = 1;
            tileGrid.PaddingBottom = 1;
            tileGrid.VerticalFit = ContentSizeFitter.FitMode.PreferredSize;
            tileGrid.SizeScaleX = 1f;

            ScrollView scrollView = new ScrollView("Scroll View", tileGrid);
            scrollView.SizeOffsetX = 0f;
            scrollView.SizeOffsetY = 0f;
            scrollView.VerticalScroll = true;
            scrollView.HorizontalScroll = false;
            scrollView.SizeScaleX = 1f;
            scrollView.SizeScaleY = 1f;

            tileWindow.GetContent().AddChild(scrollView);
            tileWindow.ShowControls = false;

            tileWindow.Hide();
            GameObject.Destroy(tileWindow.GetRootObject().GetComponent<XRGrabInteractable>());
        }

        private void OnHoverEntered(HoverEnterEventArgs e)
        {
            if (tileWindow.State == Models.Enums.WindowState.Normal)
                return;

            if (e.interactorObject.transform.parent == leftWristAnchor.transform)
            {
                Transform tileWindowTransform = tileWindow.GetRootObject().transform;
                tileWindowTransform.SetParent(leftWristAnchor.transform, false);
                tileWindowTransform.localPosition = new Vector3(0, -0.1f, 0.05f);
                tileWindowTransform.localEulerAngles = new Vector3(-90f, 0f, -180f);
                tileWindow.Show();
            }

            if (e.interactorObject.transform.parent == rightWristAnchor.transform)
            {
                Transform tileWindowTransform = tileWindow.GetRootObject().transform;
                tileWindowTransform.SetParent(rightWristAnchor.transform, false);
                tileWindowTransform.localPosition = new Vector3(0, -0.1f, 0.05f);
                tileWindowTransform.localEulerAngles = new Vector3(-90f, 0f, -180f);
                tileWindow.Show();
            }
        }

        private void OnHoverExited(HoverExitEventArgs e)
        {
            if (tileWindow.State == Models.Enums.WindowState.Hidden)
                return;

            if (e.interactorObject.transform.parent == rightWristAnchor.transform)
            {
                tileWindow.Hide();
            }

            if (e.interactorObject.transform.parent == leftWristAnchor.transform)
            {
                tileWindow.Hide();
            }
        }

        private void OnHandUpdate(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
        {
            if (updateType != XRHandSubsystem.UpdateType.BeforeRender)
                return;

            XRHand rightHand = subsystem.rightHand;
            XRHand leftHand = subsystem.leftHand;

            if (rightHand.isTracked)
            {
                rightWristAnchor.transform.position = rightHand.rootPose.position;
                rightWristAnchor.transform.rotation = rightHand.rootPose.rotation;
            }
            else
            {
                rightWristAnchor.transform.position = Vector3.zero;
            }


            if (leftHand.isTracked)
            {
                leftWristAnchor.transform.position = leftHand.rootPose.position;
                leftWristAnchor.transform.rotation = leftHand.rootPose.rotation;
            }
            else
            {
                leftWristAnchor.transform.position = Vector3.zero;
            }
        }

        public ButtonTile CreateButtonTile(string name)
        {
            if (tiles.Where(x => x.Name == name).Count() > 0)
                throw new Exception($"Tile of name '{name}' already exists!");

            ButtonTile buttonTile = new ButtonTile(name);
            tileGrid.AddChild(buttonTile);

            tiles.Add(buttonTile);
            return buttonTile;
        }

        public SwitchTile CreateSwitchTile(string name)
        {
            if (tiles.Where(x => x.Name == name).Count() > 0)
                throw new Exception($"Tile of name '{name}' already exists!");

            SwitchTile toggleTile = new SwitchTile(name);
            tileGrid.AddChild(toggleTile);

            tiles.Add(toggleTile);
            return toggleTile;
        }

        public Control GetTileByName(string name)
        {
            Control tile = tiles.Where(x => x.Name == name).FirstOrDefault();
            return tile;
        }

        private void UpdateProvider_OnLateUpdate(object sender, EventArgs e)
        {
            tileWindow.Update();
        }
    }
}
