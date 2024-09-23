using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Demo3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Demo3;

public partial class EditWindow : Window
{
    int index = -1;
    List<string> manf = Actions.PublicContext.Manufacturers.Select(m => m.Name).ToList();
    string _imageName = "";
    string _imagePath = "";
    public List<Product> ExtraProducts = new List<Product>();
    string cost = "";
    public EditWindow()
    {
        InitializeComponent();
        Man.ItemsSource = manf;
        Available.ItemsSource = Actions.PublicContext.Products.Where(p => p.Isactive == 1);
    }
    public EditWindow(int id)
    {
        index = id;
        InitializeComponent();
        Man.ItemsSource = manf;

        Product product = Actions.PublicContext.Products.FirstOrDefault(p => p.Id == index);

        Id.Text = id.ToString();
        Name.Text = product.Title;
        Cost.Text = product.Cost.ToString();
        Desc.Text = product.Description;
        Image.Source = product.Image;
        Man.SelectedIndex = manf.IndexOf(Actions.PublicContext.Manufacturers.FirstOrDefault(m => m.Id == product.Manufacturerid).Name);
        ExtraProducts = Actions.Products.FirstOrDefault(p => p.Id == id).Attachedproducts.ToList();
        Extra.ItemsSource = ExtraProducts;
        Available.ItemsSource = Actions.PublicContext.Products.Where(p => p.Isactive == 1 && p.Id!= index);
    }

    private void Comfirm(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Name.Text) && !string.IsNullOrEmpty(Cost.Text) && Man.SelectedIndex != null)
        {
            if (index == -1)
            {
                Product product = new Product() { Title = Name.Text, Cost = decimal.Parse(Cost.Text), Description = Desc.Text, 
                    Manufacturerid = Actions.PublicContext.Manufacturers.FirstOrDefault(m => m.Name == manf[Man.SelectedIndex]).Id, 
                    Isactive = (Convert.ToInt32(IsActive.IsChecked) + 1) * -1 + 3, Mainimagepath = _imageName
                };
            
                if (_imagePath != "")
                    File.Copy(_imagePath, Environment.CurrentDirectory + "/" + _imageName);
                Actions.PublicContext.Products.Add(product);
                Actions.PublicContext.SaveChanges();
            }
            else
            {
                Product product = Actions.PublicContext.Products.FirstOrDefault(p => p.Id == index);
                product.Title  = Name.Text;
                product.Cost = decimal.Parse(Cost.Text);
                product.Description  = Desc.Text;
                product.Manufacturerid = Actions.PublicContext.Manufacturers.FirstOrDefault(m => m.Name == manf[Man.SelectedIndex]).Id;
                product.Mainimagepath = _imageName;
                product.Isactive = (Convert.ToInt32(IsActive.IsChecked) + 1) * -1 + 3;
                if (_imagePath != "")
                    File.Copy(_imagePath, Environment.CurrentDirectory + "/" + _imageName);
                Actions.PublicContext.Products.Update(product);
                Actions.PublicContext.SaveChanges();
            }
            new MainWindow().Show();
            this.Close();
        }
    }

    private async void AddImage(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions { Title="Выберите изображение"});
        if (files.Count() != 0)
        {
            try
            {
                _imagePath = files[0].Path.LocalPath;
                Image.Source = new Bitmap(_imagePath);
                _imageName = $"SchoolSupplies/{Guid.NewGuid()}{_imagePath.Substring(_imagePath.LastIndexOf('.'), _imagePath.Length - _imagePath.LastIndexOf('.'))}";
            }
            catch { }
        }
    }

    private void CostTextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if ((float.TryParse((sender as TextBox).Text, out float result) && result > 0) || string.IsNullOrEmpty((sender as TextBox).Text))
        {
            cost = (sender as TextBox).Text;
        }
        else
        {
            (sender as TextBox).Text = cost;
        }
    }
}