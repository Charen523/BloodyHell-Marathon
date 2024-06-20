public class EntityStateMachine : StateMachine
{
  public Entity Entity { get; private set; }
  public float MovementSpeedModifier { get; set; }

  public EntityStateMachine(Entity entity)
  {
    Entity = entity;
  }
}