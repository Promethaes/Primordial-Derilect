using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class SingleBulletFire : MonoBehaviour
{

    public GameObject bulletPrefab;
    public GunScriptableObject gunInfo;
    List<Rigidbody> bullets = new List<Rigidbody>();
    public Transform bulletStartPosition;
    public List<Transform> bulletSpreadStartPositions = new List<Transform>();
    int bulletKey = 0;

    //Sounds
    public StudioEventEmitter shootSound;

    // Start is called before the first frame update
    void Start()
    {
        //fixed bullet size, can just use object pooling to prevent large
        //instantiation. 10 seems like a decent size
        for (int i = 0; i < 20; i++)
        {
            var bullet = GameObject.Instantiate(bulletPrefab);
            if (gunInfo.bulletHasArc)
                bullet.GetComponent<Rigidbody>().useGravity = true;
            bullets.Add(bullet.GetComponent<Rigidbody>());
        }
    }

    bool canFire = true;
    IEnumerator Cooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(1.0f / gunInfo.rateOfFire);
        canFire = true;
    }

    private void OnDisable()
    {
        canFire = true;
    }

    public void Shoot()
    {
        if (!gameObject.activeSelf || !canFire)
            return;
        shootSound.Play();
        if (gunInfo.firesInSpread)
        {
            //NOTE: negative on the forward vec cause the guns are all backwards
            for (int i = 0; i < gunInfo.bulletsInSpread; i++)
            {
                bullets[bulletKey].velocity = Vector3.zero;
                bullets[bulletKey].angularVelocity = Vector3.zero;
                bullets[bulletKey].gameObject.SetActive(true);
                bullets[bulletKey].gameObject.transform.position = bulletSpreadStartPositions[i].position;

                var direction = (bullets[bulletKey].transform.position - bulletStartPosition.position).normalized;

                bullets[bulletKey].AddForce((-gameObject.transform.forward*gunInfo.spreadTightness + direction).normalized * gunInfo.bulletSpeed);
                bulletKey = (bulletKey + 1) % bullets.Count;
            }

            StartCoroutine("Cooldown");

            return;

        }

        bullets[bulletKey].velocity = Vector3.zero;
        bullets[bulletKey].angularVelocity = Vector3.zero;
        bullets[bulletKey].gameObject.SetActive(true);
        bullets[bulletKey].gameObject.transform.position = bulletStartPosition.position;
        bullets[bulletKey].AddForce(-gameObject.transform.forward * gunInfo.bulletSpeed);
        bulletKey = (bulletKey + 1) % bullets.Count;
        StartCoroutine("Cooldown");
    }
}
