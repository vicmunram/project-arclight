using UnityEngine;

public class RespawnPoint : Interactable
{
    public override void FirstInteraction() { }
    public override void EveryInteraction()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerControl>().SetRespawnPoint(new Vector3(transform.position.x, transform.position.y, player.transform.position.z));
    }

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }
}
