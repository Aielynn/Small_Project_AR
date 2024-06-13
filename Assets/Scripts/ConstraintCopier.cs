using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ConstraintCopier : MonoBehaviour
{
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    public void SetParentConstraint(GameObject prop)
    {
        ParentConstraint outlineParentConstraint = this.GetComponent<ParentConstraint>();
        ParentConstraint propParentConstraint = prop.AddComponent<ParentConstraint>();

        // Copy constraint settings
        propParentConstraint.constraintActive = outlineParentConstraint.constraintActive;
        propParentConstraint.weight = outlineParentConstraint.weight;

        ConstraintSource outlineConstrSource = outlineParentConstraint.GetSource(0);
        propParentConstraint.AddSource(outlineConstrSource);

        //propParentConstraint.SetSourceWeight(0, outlineConstrSource.weight);
        propParentConstraint.SetTranslationOffset(0, positionOffset);
        propParentConstraint.SetRotationOffset(0, rotationOffset);

        propParentConstraint.locked = true;
    }
}
