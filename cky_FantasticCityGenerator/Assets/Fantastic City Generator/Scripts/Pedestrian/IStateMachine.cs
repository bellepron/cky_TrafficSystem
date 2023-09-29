
using cky.FCG.Pedestrian.StateMachine;

namespace cky.FCG.Pedestrian
{
    public interface IStateMachine
    {
        public PedestrianStates State { get; set; }
        public PedestrianStates PreSet_State { get; set; }
        public bool IsInsideSemaphore { get; set; }
        public bool IsRedSemaphore { get; set; }
        void ChangeState(PedestrianStates state);
        void ArrivedToTaxiDestination();
    }
}