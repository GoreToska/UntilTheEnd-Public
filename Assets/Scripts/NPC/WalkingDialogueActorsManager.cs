using UnityEngine.AI;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;

public class WalkingDialogueActorsManager : MonoBehaviour
{
    [HideInInspector] public static WalkingDialogueActorsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

	private void Start()
	{
		//DontDestroyOnLoad(gameObject);
	}

	public void RegisterLua()
    {
        Lua.RegisterFunction("DisableActorWalk", this, SymbolExtensions.GetMethodInfo(() => DisableActorWalk((string)"", (double)0)));
        Lua.RegisterFunction("LetActorWalk", this, SymbolExtensions.GetMethodInfo(() => LetActorWalk((string)"", (double)0)));
        Lua.RegisterFunction("Rotate", this, SymbolExtensions.GetMethodInfo(() => Rotate((float)0f, (string)"")));
    }

    public void UnregisterLua()
    {
        Lua.UnregisterFunction("DisableActorWalk");
        Lua.UnregisterFunction("LetActorWalk");
        Lua.UnregisterFunction("Rotate");
	}

    public void Rotate(float value, string name)
    {
        var actor = GameObject.Find(name);

        if (actor == null)
        {
            Debug.LogError($"{name} is not found");
            return;
        }

        StartCoroutine(Rotate((float)value, actor));
        Debug.Log("Rotate");

    }

    public void LetActorWalk(string name, double seconds)
    {
        var actor = GameObject.Find(name);

        if (actor == null)
        {
            Debug.LogError($"{name} is not found");
            return;
        }

        StartCoroutine(WaitAndActivate((float)seconds, actor));
    }

    private IEnumerator WaitAndActivate(float seconds, GameObject actor)
    {
        yield return new WaitForSeconds(seconds);
        actor.GetComponent<NavMeshObstacle>().enabled = false;
        actor.GetComponent<NavMeshAgent>().enabled = true;

        yield return null;
    }

    public void DisableActorWalk(string name, double seconds)
    {
        var actor = GameObject.Find(name);

        if (actor == null)
        {
            Debug.LogError($"{name} is not found");
            return;
        }

        StartCoroutine(WaitAndDisable((float)seconds, actor));
    }

    private IEnumerator WaitAndDisable(float seconds, GameObject actor)
    {
        yield return new WaitForSeconds(seconds);
        actor.GetComponent<NavMeshAgent>().enabled = false;
        actor.GetComponent<NavMeshObstacle>().enabled = true;

        yield return null;
    }

    IEnumerator Rotate(float value, GameObject actor)
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(actor.transform.rotation.x, actor.transform.rotation.y + value, actor.transform.rotation.z));
        float time = 0;

        while (time < 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time / 0.5f);

            time += Time.deltaTime;

            yield return null;
        }
    }
}
