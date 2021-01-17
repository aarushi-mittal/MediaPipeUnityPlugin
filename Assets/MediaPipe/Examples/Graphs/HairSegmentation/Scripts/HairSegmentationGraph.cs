using Mediapipe;
using UnityEngine;

public class HairSegmentationGraph : DemoGraph {
  private const string hairMaskStream = "hair_mask_cpu";
  private OutputStreamPoller<ImageFrame> hairMaskStreamPoller;
  private ImageFramePacket hairMaskPacket;

  public override Status StartRun() {
    hairMaskStreamPoller = graph.AddOutputStreamPoller<ImageFrame>(hairMaskStream).ConsumeValueOrDie();
    hairMaskPacket = new ImageFramePacket();

    return graph.StartRun();
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var hairMask = FetchNextHairMask();
    var texture = screenController.GetScreen();

    texture.SetPixels32(textureFrame.GetPixels32());
    RenderAnnotation(screenController, hairMask);

    texture.Apply();
  }

  private ImageFrame FetchNextHairMask() {
    return FetchNext(hairMaskStreamPoller, hairMaskPacket, hairMaskStream);
  }

  private void RenderAnnotation(WebCamScreenController screenController, ImageFrame hairMask) {
    // NOTE: input image is flipped
    GetComponent<MaskAnnotationController>().Draw(screenController.GetScreen(), hairMask, new Color(0, 0, 255), true);
  }
}