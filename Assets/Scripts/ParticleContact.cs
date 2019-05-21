using UnityEngine;
using cyclone;
using Vector3 = cyclone.Vector3;

public class ParticleContact
{
    public Particle particle;
    private float restitution;
    public Vector3 contactNormal = new Vector3(0, 0, 0);
    public float penetration;

    public ParticleContact(Particle particle, float restitution, Vector3 contactNormal)
    {
        this.particle = particle;
        this.restitution = restitution;
        this.contactNormal = contactNormal;
    }

    public void Resolve(float duration)
    {
        ResolveVelocity(duration);
        ResolveInterpenetration(duration);
    }

    private float CalculateSeparatingVelocity()
    {
        Vector3 velocity = particle.GetVelocity();
        return velocity.ScalarProduct(contactNormal);
    }

    private void ResolveVelocity(float duration)
    {
        float separatingVelocity = CalculateSeparatingVelocity();

        if (-separatingVelocity > 0)
        {
            return;
        }

        float newSeparatingVelocity = -separatingVelocity * restitution;

        Vector3 accelerationCausedVelocity = particle.GetAcceleration();

        float accelerationCausedSeparatingVelocity = accelerationCausedVelocity.ScalarProduct(contactNormal) * duration;

        if(accelerationCausedSeparatingVelocity < 0)
        {
            newSeparatingVelocity += restitution * accelerationCausedSeparatingVelocity;
            if(newSeparatingVelocity < 0)
            {
                newSeparatingVelocity = 0;
            }
        }

        float deltaVelocity = newSeparatingVelocity - separatingVelocity;
        Vector3 impulse = contactNormal * deltaVelocity;
        Vector3 totalVelocity = particle.GetVelocity() + impulse;
        particle.SetVelocity(totalVelocity.x, totalVelocity.y, totalVelocity.z);
    }

    private void ResolveInterpenetration(float duration)
    {
        if(penetration <= 0)
        {
            return;
        }
        Vector3 move = contactNormal * -penetration;
        Vector3 finalPosition = particle.GetPosition() + move;
        particle.SetPosition(finalPosition.x, finalPosition.y, finalPosition.z);
    }
}
