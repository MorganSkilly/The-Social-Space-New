﻿using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    public sealed class TubeDistortionRenderer : PostProcessEffectRenderer<TubeDistortion>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.TubeDistortion);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat("_Amount", settings.amount.value);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.mode.value);
        }
    }
#endif
}