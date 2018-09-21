namespace Constants
{
    public static class Others
    {
        public static float WireWidth { get; } = .15f;
        public static float WireHeight { get; } = -.01f;
        public static float TimeForDoubleTap { get; } = .5f;
        public static float KnobRotationAngle { get; } = 270f;
        public static float KnobTouchAmplitude { get; } = 200f;
        public static float CameraMinSize { get; } = 3f;
        public static float CameraMaxSize { get; } = 30f;
        public static float CameraMaxPanX{ get; } = 50f;
        public static float CameraMaxPanY { get; } = 50f;
        public static float WaveWidth { get; } = 0.007f;
        public static float SaveFilesButtonHeigth { get; } = 50f;
        public static int InterpolationPoints { get; } = 10;
        public static int WavePoints { get; } = 2048;
        public static string PatchBodiesPrefabsFolder { get; } = "Prefabs/PatchBodies/";
        public static string PatchEditPrefabsFolder { get; } = "Prefabs/PatchEdit/";
        public static string SavesFolder { get; } = "/Saves/";
    }
}