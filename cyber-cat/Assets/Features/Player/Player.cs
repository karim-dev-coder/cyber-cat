﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Zenject;

public class Player : MonoBehaviour
{
    private static Scene AuthorizationScene;
    private static Scene CodeEditorScene;

    public static bool CanInput
    {
        get
        {
            var scene = SceneManager.GetActiveScene();
            return scene != AuthorizationScene && scene != CodeEditorScene;
        }
    }

    public PlayerInteractHandler Interact { get; private set; }

    private const float _moveSpeed = 4f;

    private NavMeshAgent _navMeshAgent;


    private void Awake()
    {
        TryGetComponent(out _navMeshAgent);
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            switch (scene.name)
            {
                case "AuthorizationScene":
                    AuthorizationScene = scene;
                    break;
                case "CodeEditor":
                    CodeEditorScene = scene;
                    break;
            }
        }
    }

    [Inject]
    public void Construct(PlayerInteractHandler interactHandler)
    {
        Interact = interactHandler;
    }

    private void Update()
    {
        if (!CanInput)
            return;

        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        var directionVector = new Vector3(horizontal, 0, vertical);
        _navMeshAgent.velocity = Vector3.ClampMagnitude(directionVector, 1) * _moveSpeed;

        Interact.OnUpdate();
    }
}