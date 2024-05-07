using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using BuildDeployWpf.Views;

namespace BuildDeployWpf.Utils;

internal interface IAnimationManager<in T>
{
    void CreateAnimation(T from, T to, Storyboard storyboard,
        DependencyObject target, PropertyPath propertyTarget, double duration);
}

internal class DoubleAnimationManager : IAnimationManager<double>
{
    public void CreateAnimation(double from, double to, Storyboard storyboard, DependencyObject target,
        PropertyPath propertyTarget, double duration)
    {
        var doubleAnimation = new DoubleAnimation
        {
            From = from,
            To = to,
            Duration = TimeSpan.FromSeconds(duration)
        };
        storyboard.Children.Add(doubleAnimation);
        Storyboard.SetTarget(doubleAnimation, target);
        Storyboard.SetTargetProperty(doubleAnimation, propertyTarget);
    }
}

internal class GridLengthAnimationManager : IAnimationManager<GridLength>
{
    public void CreateAnimation(GridLength from, GridLength to, Storyboard storyboard, DependencyObject target,
        PropertyPath propertyTarget, double duration)
    {
        var doubleAnimation = new GridLengthAnimation
        {
            From = from,
            To = to,
            Duration = TimeSpan.FromSeconds(duration)
        };
        storyboard.Children.Add(doubleAnimation);
        Storyboard.SetTarget(doubleAnimation, target);
        Storyboard.SetTargetProperty(doubleAnimation, propertyTarget);
    }
}

internal static class AnimationManager
{
    private const double AnimationDuration = 0.2;

    public static void CreateDoubleAnimation(double from, double to, Storyboard storyboard, DependencyObject target,
        PropertyPath propertyTarget)
    {
        IAnimationManager<double> manager = new DoubleAnimationManager();
        manager.CreateAnimation(from, to, storyboard, target, propertyTarget, AnimationDuration);
    }

    public static void CreateGridLengthAnimation(GridLength from, GridLength to, Storyboard storyboard,
        DependencyObject target,
        PropertyPath propertyTarget)
    {
        IAnimationManager<GridLength> manager = new GridLengthAnimationManager();
        manager.CreateAnimation(from, to, storyboard, target, propertyTarget, AnimationDuration);
    }
}