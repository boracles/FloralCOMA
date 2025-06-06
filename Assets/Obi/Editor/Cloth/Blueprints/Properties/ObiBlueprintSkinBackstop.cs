using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Obi
{
    public class ObiBlueprintSkinBackstop : ObiBlueprintFloatProperty
    {

        public ObiBlueprintSkinBackstop(ObiActorBlueprintEditor editor) : base(editor)
        {
            brushModes.Add(new ObiFloatPaintBrushMode(this));
            brushModes.Add(new ObiFloatAddBrushMode(this));
            brushModes.Add(new ObiFloatCopyBrushMode(this, this));
            brushModes.Add(new ObiFloatSmoothBrushMode(this));
        }

        public override string name
        {
            get { return "Skin backstop"; }
        }

        public override float Get(int index)
        {
            var constraints = editor.blueprint.GetConstraintsByType(Oni.ConstraintType.Skin) as ObiConstraints<ObiSkinConstraintsBatch>;
            return constraints.batches[0].skinRadiiBackstop[index * 3 + 2];
        }
        public override void Set(int index, float value)
        {
            var constraints = editor.blueprint.GetConstraintsByType(Oni.ConstraintType.Skin) as ObiConstraints<ObiSkinConstraintsBatch>;
            constraints.batches[0].skinRadiiBackstop[index * 3 + 2] = value;
            editor.blueprint.edited = true;
        }
        public override bool Masked(int index)
        {
            return !editor.Editable(index);
        }

        public override void OnSceneRepaint()
        {
            var meshEditor = editor as ObiMeshBasedActorBlueprintEditor;
            if (meshEditor != null)
            {
                using (new Handles.DrawingScope(Color.yellow, Matrix4x4.identity))
                {
                    var constraints = meshEditor.blueprint.GetConstraintsByType(Oni.ConstraintType.Skin) as ObiConstraints<ObiSkinConstraintsBatch>;
                    if (constraints != null)
                    {
                        var batches = constraints.batches;
                        foreach (ObiSkinConstraintsBatch batch in batches)
                        {
                            for (int i = 0; i < batch.activeConstraintCount; ++i)
                            {
                                int particleIndex = batch.particleIndices[i];
                                if (meshEditor.visible[particleIndex])
                                {
                                    Vector3 position = meshEditor.blueprint.GetParticlePosition(particleIndex);
                                    Quaternion restOrientation = meshEditor.blueprint.GetParticleRestOrientation(particleIndex);
                                    Handles.DrawLine(position, position - restOrientation * Vector3.forward * batch.skinRadiiBackstop[i * 3+2]);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
