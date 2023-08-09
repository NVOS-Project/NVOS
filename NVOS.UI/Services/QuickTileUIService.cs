using NVOS.Core;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Management;

namespace NVOS.UI.Services
{
    [ServiceType(ServiceType.Singleton)]
    public class QuickTileUIService : IService, IDisposable
    {
        private GameObject rightWristAnchor;
        private GameObject leftWristAnchor;

        private GameObject tickerObject;
        private GameTickProvider gameTickProvider;

        private Window3D tileWindow;

        public bool Init()
        {
            tickerObject = new GameObject("WristUITicker");
            gameTickProvider = tickerObject.AddComponent<GameTickProvider>();
            gameTickProvider.OnLateUpdate += GameTickProvider_OnLateUpdate;

            leftWristAnchor = new GameObject("Left Wrist");
            rightWristAnchor = new GameObject("Right Wrist");

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
            interactableObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            interactableObject.AddComponent<SphereCollider>().radius = 0.5f;

            XRSimpleInteractable gazeInteractable = interactableObject.AddComponent<XRSimpleInteractable>();
            gazeInteractable.allowGazeInteraction = true;

            gazeInteractable.hoverEntered.AddListener(new UnityEngine.Events.UnityAction<HoverEnterEventArgs>(OnHoverEntered));
            gazeInteractable.hoverExited.AddListener(new UnityEngine.Events.UnityAction<HoverExitEventArgs>(OnHoverExited));

            XRHandSubsystem handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
            handSubsystem.updatedHands += OnHandUpdate;

            return true;
        }

        private void WristWindowSetup()
        {
            tileWindow = new Window3D("Quick Tiles", 10f, 10f);
            tileWindow.Hide();
            tileWindow.GetContent().BackgroundColor = Color.gray;

            tileWindow.ShowControls = false;
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
                tileWindowTransform.localPosition = new Vector3(-0.1f, -0.1f, 0.05f);
                tileWindowTransform.localEulerAngles = new Vector3(-90f, 0f, -180f);
                tileWindow.Show();
            }

            if (e.interactorObject.transform.parent == rightWristAnchor.transform)
            {
                Transform tileWindowTransform = tileWindow.GetRootObject().transform;
                tileWindowTransform.SetParent(rightWristAnchor.transform, false);
                tileWindowTransform.localPosition = new Vector3(0.1f, -0.1f, 0.05f);
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

        public ButtonTile CreateButtonTile()
        {
            throw new NotImplementedException();
        }

        public ToggleTile CreateToggleTile()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        private void GameTickProvider_OnLateUpdate(object sender, EventArgs e)
        {
            tileWindow.Update();
        }
    }
}
