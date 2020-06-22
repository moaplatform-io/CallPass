namespace Kons.Utility
{
    public interface IKnRecycleObj<TObj>
    {
        void initObj();
        void copyObj(TObj src);
    }
}
