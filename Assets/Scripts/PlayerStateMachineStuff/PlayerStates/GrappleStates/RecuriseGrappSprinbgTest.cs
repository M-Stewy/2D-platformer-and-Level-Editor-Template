using System.Collections.Generic;
using UnityEngine;

public class RecuriseGrappSprinbgTest : MonoBehaviour
{
    [SerializeField] bool testingSpring;
    [SerializeField] bool testingDist;
    [SerializeField] bool testingHinge;

    [SerializeField] GameObject SrpingPreF;
    [SerializeField] int springLength;
    [SerializeField] List<GameObject> allJoints = new List<GameObject>();
    [Space]
    [SerializeField] float dist;
    [SerializeField][Range(0,1)] float DampR;
    [SerializeField] float Freq;

    private SpringJoint2D sprgJ;
    private DistanceJoint2D disJ;
    private HingeJoint2D hingJ;

   public void CreateSrpingJointPoint(Transform startPos)
   {
        if(testingSpring)
        {
            allJoints.Clear();

            for (int i = 0; i < springLength; i++)
            {

                if (i == 0)
                {
                    GameObject joint = Instantiate(SrpingPreF, startPos);
                    sprgJ = joint.GetComponent<SpringJoint2D>();
                    sprgJ.connectedBody = startPos.GetComponent<Rigidbody2D>();
                    allJoints.Add(joint);
                }
                else
                {
                    GameObject joint = Instantiate(SrpingPreF, allJoints[i - 1].transform);
                    sprgJ = joint.GetComponent<SpringJoint2D>();
                    sprgJ.connectedBody = allJoints[i - 1].GetComponent<Rigidbody2D>();
                    allJoints.Add(joint);
                }
                sprgJ.distance = dist;
                sprgJ.dampingRatio = DampR;
                sprgJ.frequency = Freq;

            }
        }
        if(testingDist)
        {
            allJoints.Clear();

            for (int i = 0; i < springLength; i++)
            {

                if (i == 0)
                {
                    GameObject joint = Instantiate(SrpingPreF, startPos.transform.position, startPos.transform.rotation, startPos);
                    
                    disJ = joint.GetComponent<DistanceJoint2D>();
                    disJ.connectedBody = startPos.GetComponent<Rigidbody2D>();
                    allJoints.Add(joint);
                }
                else
                {
                    GameObject joint = Instantiate(SrpingPreF, allJoints[i - 1].transform.position, allJoints[i - 1].transform.rotation, allJoints[i - 1].transform);
                    
                    disJ = joint.GetComponent<DistanceJoint2D>();
                    disJ.connectedBody = allJoints[i - 1].GetComponent<Rigidbody2D>();
                    allJoints.Add(joint);
                }
                disJ.distance = dist;

            }
        }
        if (testingHinge)
        {
            allJoints.Clear();

            for (int i = 0; i < springLength; i++)
            {

                if (i == 0)
                {
                    GameObject joint = Instantiate(SrpingPreF, startPos.transform.position, startPos.transform.rotation, transform);

                    hingJ = joint.GetComponent<HingeJoint2D>();
                    hingJ.connectedBody = startPos.GetComponent<Rigidbody2D>();
                    allJoints.Add(joint);
                }
                else
                {
                    GameObject joint = Instantiate(SrpingPreF, allJoints[i - 1].transform.position - new Vector3(0,dist,0), allJoints[i - 1].transform.rotation, transform);

                    hingJ = joint.GetComponent<HingeJoint2D>();
                    hingJ.connectedBody = allJoints[i - 1].GetComponent<Rigidbody2D>();
                    allJoints.Add(joint);
                }

            }

        }
        
   }
}
