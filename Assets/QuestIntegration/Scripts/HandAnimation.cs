using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Animating the hands based on input values
public class HandAnimation : MonoBehaviour
{
    private int m_animLayerIndexThumb = -1;
    private int m_animLayerIndexPoint = -1;
    private int m_animParamIndexFlex = -1;
    Animator m_animator = null;

    public bool isRight;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_animLayerIndexPoint = m_animator.GetLayerIndex("Point Layer");
        m_animLayerIndexThumb = m_animator.GetLayerIndex("Thumb Layer");
        m_animParamIndexFlex = Animator.StringToHash("Flex");
    }

    // Update is called once per frame
    void Update()
    {
        m_animator.SetLayerWeight(m_animLayerIndexPoint, (isRight ? QuestInputs.Right.Trigger.Current : QuestInputs.Left.Trigger.Current) ? 0.0f : 1.0f);
        m_animator.SetFloat(m_animParamIndexFlex, (isRight ? QuestInputs.Right.Grip.Current : QuestInputs.Left.Grip.Current) ? 0.2f : 0.0f);

        bool condition = isRight ? (QuestInputs.Right.Button1.Current || QuestInputs.Right.Button2.Current) : (QuestInputs.Left.Button1.Current || QuestInputs.Left.Button2.Current);
        m_animator.SetLayerWeight(m_animLayerIndexThumb, (condition ? 0.0f : 0.2f));
    }
}
