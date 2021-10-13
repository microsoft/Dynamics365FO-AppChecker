using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.LinkLabel;

#nullable enable

namespace GraphExplorer.ViewModels
{
    public class IconViewModel : ViewModelBase
    {
        private readonly Lazy<IEnumerable<GraphExplorer.Models.IconKindGroup>> _packIconKinds;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private readonly IDictionary<PackIconKind, string> factory = GraphExplorer.Models.PackIconDataFactory.Create();

        public IconViewModel()
        {
            // this._snackbarMessageQueue = snackbarMessageQueue ?? throw new ArgumentNullException(nameof(snackbarMessageQueue));

            this.SearchCommand = new RelayCommand(this.Search);

            this._packIconKinds = new Lazy<IEnumerable<GraphExplorer.Models.IconKindGroup>>(() =>
                Enum.GetNames(typeof(PackIconKind))
                    .GroupBy(k => (PackIconKind)Enum.Parse(typeof(PackIconKind), k))
                    .Select(g => new GraphExplorer.Models.IconKindGroup(g))
                    .OrderBy(x => x.Kind)
                    .ToList());

            // Fill in the Paths to be used when rendering.
            foreach (var pi in this._packIconKinds.Value)
            {
                var kind =(PackIconKind)Enum.Parse(typeof(PackIconKind), pi.Kind);
                if (factory.ContainsKey(kind))
                {
                    pi.Path = factory[kind];
                }
            }

            var helper = new PaletteHelper();
            if (helper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += this.ThemeManager_ThemeChanged;
            }
            this.SetDefaultIconColors();
        }

        private void SetDefaultIconColors()
        {
            var helper = new PaletteHelper();
            ITheme theme = helper.GetTheme();
            GeneratedIconBackground = theme.Paper;
            GeneratedIconForeground = theme.PrimaryMid.Color;
        }

        private Color _generatedIconBackground;
        public Color GeneratedIconBackground
        {
            get => _generatedIconBackground;
            set => SetProperty(ref _generatedIconBackground, value);
        }

        private Color _generatedIconForeground;
        public Color GeneratedIconForeground
        {
            get => _generatedIconForeground;
            set => SetProperty(ref _generatedIconForeground, value);
        }

        private void ThemeManager_ThemeChanged(object? sender, ThemeChangedEventArgs e)
            => this.SetDefaultIconColors();

        public ICommand SearchCommand { get; }


        private IEnumerable<GraphExplorer.Models.IconKindGroup>? _kinds;
        private GraphExplorer.Models.IconKindGroup? _group;
        private string? _kind;
        private PackIconKind _packIconKind;

        public IEnumerable<GraphExplorer.Models.IconKindGroup> Kinds
        {
            get => this._kinds ??= this._packIconKinds.Value;
            set => this.SetProperty(ref this._kinds, value);
        }

        public GraphExplorer.Models.IconKindGroup? Group
        {
            get => this._group;
            set
            {
                if (this.SetProperty(ref this._group, value))
                {
                    this.Kind = value?.Kind;
                }
            }
        }

        public string? Kind
        {
            get => this._kind;
            set
            {
                if (this.SetProperty(ref this._kind, value))
                {
                    this.IconKind = value != null ? (PackIconKind)Enum.Parse(typeof(PackIconKind), value) : default;
                }
            }
        }

        public PackIconKind IconKind
        {
            get => this._packIconKind;
            set => this.SetProperty(ref this._packIconKind, value);
        }

        public string Path { get => this.factory[this.IconKind]; }

        private async void Search(object obj)
        {
            var text = obj as string;
            if (string.IsNullOrWhiteSpace(text))
            {
                this.Kinds = this._packIconKinds.Value;
            }
            else
            {
                this.Kinds = await Task.Run(() => this._packIconKinds.Value
                    .Where(x => x.Aliases.Any(a => a.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0))
                    .ToList());
            }
        }
    }
}


