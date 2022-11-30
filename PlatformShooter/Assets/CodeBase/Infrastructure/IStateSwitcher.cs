namespace Assets.CodeBase.Infrastructure
{
    public interface IStateSwitcher
    {
        public void SwitchState<TState>() where TState : IState;
    }
}
