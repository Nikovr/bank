namespace Banks
{
    public interface ITimeTravel
    {
        void AddDays(int days);
        void AddMonths(int months);
        void AddYears(int years);
    }
}
