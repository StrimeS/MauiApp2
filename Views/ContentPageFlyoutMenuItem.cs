namespace MauiApp1.Views
{
    public class ContentPageFlyoutMenuItem
    {
        public ContentPageFlyoutMenuItem()
        {
            TargetType = typeof(ContentPageFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
