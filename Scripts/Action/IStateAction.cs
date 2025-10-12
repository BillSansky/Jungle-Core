namespace Jungle.Actions
{
    public interface IStateAction
    {
        
        void OnStateEnter();
        
        void OnStateExit();
    }
}