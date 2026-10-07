using System;
using UniRx;
using UnityEngine;
using Zenject;

// 입력 -> Model, Model, View 판단 로직 없이 연결만
public class HotbarPresenter : IInitializable, IDisposable
{
    [Inject] private HotbarModel model;
    [Inject] private HotbarView view;
    [Inject] private PlayerInputHandler input;

    private readonly CompositeDisposable disposables = new CompositeDisposable();

    public void Initialize()
    {
        input.HotbarKey
            .Subscribe(model.Select)
            .AddTo(disposables);
        input.HotbarScroll
            .Subscribe(model.Scroll)
            .AddTo(disposables);

        model.SelectedIndex
            .Subscribe(view.Highlight)
            .AddTo(disposables);
    }
    public void Dispose() => disposables.Dispose();
}
