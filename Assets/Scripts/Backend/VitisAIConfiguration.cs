using System;
using System.Collections.Generic;

namespace MilehighWorld.Backend
{
    [Serializable]
    public class PreprocessConfig
    {
        public string preprocess_type = "letterbox";
        public string colour_format = "BGR";
        public int Height = 224;
        public int Width = 224;
        public int qt_fctr = 64;
        public float mean_r = 104.0f;
        public float mean_g = 117.0f;
        public float mean_b = 123.0f;
        public float scale_r = 1.0f;
        public float scale_g = 1.0f;
        public float scale_b = 1.0f;
        public bool symmetric_padding = true;
    }

    [Serializable]
    public class PostprocessConfig
    {
        public string labels_path = "/etc/vai/labels/labels";
        public float score_threshold = 0.3f;
    }

    [Serializable]
    public class MetaconvertConfig
    {
        public string kernel_name = "metaconvert_ihls";
        public int size = 4096;
    }

    [Serializable]
    public class VitisAIConfig
    {
        public string xclbin_location = "/run/media/mmcblk0p1/dpu.xclbin";
        public PreprocessConfig preprocess_config = new PreprocessConfig();
        public PostprocessConfig postprocess_config = new PostprocessConfig();
        public MetaconvertConfig metaconvert_config = new MetaconvertConfig();
    }
}
