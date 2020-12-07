using UnityEngine;
using Conf;
using System.Collections;

namespace Core
{
    public class Passenger : MonoBehaviour
    {
        public Country Destination { get; private set; }
        public int Reward { get; private set; }

        private Animator anim;
        private Material selfmat;
        [SerializeField]private float speedAnim = 1;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            anim = GetComponentInChildren<Animator>();

            selfmat = ResourceManager.instance.GetRandomPassangerMaterial();           
            foreach (var render in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                render.material = selfmat;
            }

            Destination = ResourceManager.instance.GetRandomCountry();
            Reward = UnityEngine.Random.Range(50, 100);
        }

        public void Enjoy()
        {
            StartCoroutine("EnjoyAnim");
        }

        public void Sad()
        {
            StartCoroutine("SadAnim");
        }


        IEnumerator EnjoyAnim()
        {
            float proc = anim.GetFloat("BlendBrovi");

            while (proc != 1 && speedAnim > 0)
            {
                proc = Mathf.MoveTowards(proc, 0, speedAnim / 10);
                anim.SetFloat("BlendBrovi", proc);
                yield return new WaitForFixedUpdate();
            }
        }

        IEnumerator SadAnim()
        {
            float proc = anim.GetFloat("BlendBrovi");

            while(proc != 0 && speedAnim > 0)
            {
                proc = Mathf.MoveTowards(proc, 1, speedAnim/10);
                anim.SetFloat("BlendBrovi", proc);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

