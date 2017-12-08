using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PathfindTestAgent : MonoBehaviour
{
    public int emitCount = 10000;
    public float particleAccel = 0.1f;
    [Range(0f, 1f)]
    public float particleDamp = 0.1f;
    ParticleSystem particle;
    ParticleSystem.Particle[] particles;
    PathfindAgent[] agents;
    Color[] map;
    Color[] pathMap;
    System.Random random;
    PickTargetPoint parent;

    int width;
    int height;
    int count;
    Task updateTask;
    float deltaTime;

    // Use this for initialization
    void Start()
    {
        random = new System.Random();
        parent = GetComponentInParent<PickTargetPoint>();
        var mapTex = parent.map;
        map = mapTex.GetPixels();
        pathMap = parent.resultPixels;
        particle = GetComponent<ParticleSystem>();
        var main = particle.main;
        main.startLifetime = float.MaxValue;
        particle.Emit(emitCount);
        particles = new ParticleSystem.Particle[emitCount];
        agents = new PathfindAgent[emitCount];
        width = mapTex.width;
        height = mapTex.height;

        count = particle.GetParticles(particles);
        for (int i = 0; i < emitCount; i++) InitializeParticle(i);
        particle.SetParticles(particles, count);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            count = particle.GetParticles(particles);
            for (int i = 0; i < emitCount; i++) InitializeParticle(i);
            particle.SetParticles(particles, count);
        }
        deltaTime = Time.deltaTime;
        if (!parent.calculated) return;
        if (updateTask != null)
        {
            updateTask.Wait();
            particle.SetParticles(particles, count);
        }
        count = particle.GetParticles(particles);
        updateTask = Task.Run(() => { for (var i = 0; i < count; i++) UpdateAgent(i); });
    }

    void InitializeParticle(int index)
    {
        var particle = particles[index];

        Vector2 position = Vector2.zero;
        int count = 0;
        while (++count < 1000)
        {
            position = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            var color = map[Mathf.FloorToInt(position.y * height) * width + Mathf.FloorToInt(position.x * width)];
            if (color.r != 0) break;
        }

        particle.position = (position - Vector2.one * 0.5f) * 10;
        particles[index] = particle;
        var agent = agents[index];
        agent.speed = Vector2.zero;
        agents[index] = agent;
    }

    void UpdateAgent(int index)
    {
        var agent = agents[index];
        var particle = particles[index];

        var position = (Vector2)particle.position * 0.1f + Vector2.one * 0.5f;
        var x = Mathf.Clamp(position.x, 0, 1);
        var y = Mathf.Clamp(position.y, 0, 1);
        var accel = pathMap[Mathf.FloorToInt(x * width) + width * Mathf.FloorToInt(y * height)];
        var speed = agent.speed + -ColorToAccel(accel) * particleAccel * deltaTime;
        speed *= (1 - particleDamp);
        position += speed * deltaTime;

        agent.speed = speed;
        particle.position = (position - Vector2.one * 0.5f) * 10;

        agents[index] = agent;
        particles[index] = particle;
    }

    Vector2 ColorToAccel(Color sample)
        => new Vector2(sample.b - sample.g, 1 - 2 * sample.r);

    struct PathfindAgent
    {
        public Vector2 speed;
    }
}
