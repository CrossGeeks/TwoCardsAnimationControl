using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TwoCardsAnimationControl.Controls
{
    public class TwoCardsAnimationControl : ContentView
    {
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        int translationPosition = 60;
        private readonly Grid _contentHolder;
        bool _isInFront = true;
        public TwoCardsAnimationControl()
        {
            _contentHolder = new Grid();
            Content = _contentHolder;
            _contentHolder.Padding = new Thickness(0, translationPosition, 0, 0);
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (Parent != null)
            {
                tapGestureRecognizer.Tapped += OnTapGestureRecognizerTapped;
            }
            else
            {
                tapGestureRecognizer.Tapped -= OnTapGestureRecognizerTapped;
            }
        }
        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            if (_isInFront)
            {
                _contentHolder.RaiseChild(BackView);
                FrontView.Opacity = 0.2f;

                await Task.WhenAll(
                    FrontView.FadeTo(1, 500),
                    FrontView.TranslateTo(0, -translationPosition, 500),
                    BackView.TranslateTo(0, 0, 500),
                    BackView.ScaleTo(1, 500),
                    FrontView.ScaleTo(0.8f, 500)
                );
            }
            else
            {
                _contentHolder.RaiseChild(FrontView);
                BackView.Opacity = 0.2f;

                await Task.WhenAll(
                    BackView.FadeTo(1, 500),
                    FrontView.TranslateTo(0, 0, 500),
                    BackView.TranslateTo(0, -translationPosition, 500),
                    FrontView.ScaleTo(1, 500),
                    BackView.ScaleTo(0.8f, 500)
                );
            }

            _isInFront = !_isInFront;

        }

        public static readonly BindableProperty BackViewProperty =
        BindableProperty.Create(
            nameof(BackView),
            typeof(View),
            typeof(TwoCardsAnimationControl),
            null,
            BindingMode.Default,
            null,
            BackViewPropertyChanged);

        public View BackView
        {
            get { return (View)this.GetValue(BackViewProperty); }
            set { this.SetValue(BackViewProperty, value); }
        }

        private static void BackViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                (((TwoCardsAnimationControl)bindable).BackView).TranslationY = -((TwoCardsAnimationControl)bindable).translationPosition;
                (((TwoCardsAnimationControl)bindable).BackView).Scale = 0.8f;
                ((TwoCardsAnimationControl)bindable)._contentHolder.Children.Insert(0, ((TwoCardsAnimationControl)bindable).BackView);
            }
        }

        public static readonly BindableProperty FrontViewProperty =
        BindableProperty.Create(
            nameof(FrontView),
            typeof(View),
            typeof(TwoCardsAnimationControl),
            null,
            BindingMode.Default,
            null,
            FrontViewPropertyChanged);

        public View FrontView
        {
            get { return (View)this.GetValue(FrontViewProperty); }
            set { this.SetValue(FrontViewProperty, value); }
        }

        private static void FrontViewPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue != null)
            {
                ((TwoCardsAnimationControl)bindable)._contentHolder.Children.Add(((TwoCardsAnimationControl)bindable).FrontView);
            }
        }
    }
}
