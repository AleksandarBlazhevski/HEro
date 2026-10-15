public interface IState
{
    void onEnter();
    void update();
    void fixedUpdate();
    void onExit();
}
