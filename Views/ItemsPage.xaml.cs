using MauiApp1.ViewModels;


namespace MauiApp1.Views
{
    public partial class ItemsPage : ContentPage
    {
        private ItemsViewModel _viewModel;

        private bool SlidingPanelIsShown = false;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();

            Task.Run(AnimateBackground);
        }

        private async void AnimateBackground()
        {
            var parentAnim = new Animation();
            int animationDuration = 5000;
            var forwardAnim = new Animation(x => backGradient.AnchorY = x, 0, 1, Easing.Linear);
            var backwardAnim = new Animation(x => backGradient.AnchorY = x, 1, 0, Easing.Linear);

            parentAnim.Add(0, 0.5, forwardAnim);
            parentAnim.Add(0.5, 1, backwardAnim);

            parentAnim.Commit(backGradient, "parentAnim", 16U, (uint)animationDuration * 2, Easing.Linear, null, () => true);

            //while (true)
            //{
            //    forwardAnim.Commit(backGradient, "forwardAnim", 16U, (uint)animationDuration, Easing.Linear, null,  () => false );
            //    await Task.Delay(animationDuration);
            //    backwardAnim.Commit(backGradient, "backwardAnim", 16U, (uint)animationDuration, Easing.Linear, null, () => false);
            //    await Task.Delay(animationDuration);
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

            HideSlidingPanel();
        }

        private async void FloatingButton_Clicked(object sender, EventArgs e)
        {
            SwitchSlidingPanel();

            await AnimateFloatingButton();
        }

        private void SwitchSlidingPanel()
        {
            if (SlidingPanelIsShown)
            {
                SlidingPanel.TranslateTo(0, this.Height, 250, Easing.SinIn);
                SlidingPanelBackground.BackgroundColor = Colors.Transparent;
                SlidingPanelBackground.IsVisible = false;
                SlidingPanelBackground.InputTransparent = true;
            }
            else
            {
                SlidingPanel.TranslateTo(0, this.Height - QuickMenu.Height - 30, 250, Easing.SpringOut);
                SlidingPanelBackground.BackgroundColor =Color.FromRgba(55, 55, 55, 99);
                SlidingPanelBackground.IsVisible = true;
                SlidingPanelBackground.InputTransparent = false;
            }
            SlidingPanelIsShown = !SlidingPanelIsShown;
        }

        private async Task AnimateFloatingButton()
        {
            FloatingButton.ScaleTo(0.9, 125);
            await FloatingButton.TranslateTo(0, -5, 75);

            FloatingButton.ScaleTo(1, 125);
            await FloatingButton.TranslateTo(0, 5, 75);
        }

        private async void HideSlidingPanel()
        {
            while (this.Height == -1)
            {
                await Task.Delay(200);
                SlidingPanel.TranslationY = this.Height;
                SlidingPanelBackground.BackgroundColor = Colors.Transparent;
                SlidingPanelBackground.InputTransparent = true;
            }
            SlidingPanel.TranslationY = this.Height;
            SlidingPanelBackground.BackgroundColor = Colors.Transparent;
            SlidingPanelBackground.InputTransparent = true;


            SlidingPanelIsShown = false;
        }

        private  void SlidingPanelBackground_OnTapped(object sender, EventArgs e)
        {
            SwitchSlidingPanel();
        }

        private void QuickMenuButton_OnClicked(object sender, EventArgs e) {

            SwitchSlidingPanel();
        }

        private void SlidingPanel_OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Up:
                    SlidingPanel.TranslateTo(0, this.Height - QuickMenu.Height - 350 - 30, 250, Easing.SpringOut);

                    break;

                case SwipeDirection.Down:
                    SlidingPanel.TranslateTo(0, this.Height - QuickMenu.Height- 30, 250, Easing.SpringOut);

                    break;
            }
        }
    }
}