namespace NVOS.UI.Models.EventArgs
{
    public class SliderValueChangedEventArgs : System.EventArgs
    {
        public float Value;

        public SliderValueChangedEventArgs(float value)
        {
            Value = value;
        }
    }
}
