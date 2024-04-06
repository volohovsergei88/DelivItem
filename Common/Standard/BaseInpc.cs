﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Standard
{
    /// <summary>Базовый класс с реализацией <see cref="INotifyPropertyChanged"/>.</summary>
    public abstract partial class BaseInpc : INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Защищённый метод для создания события <see cref="PropertyChanged"/>.</summary>
        /// <param name="propertyName">Имя изменившегося свойства. 
        /// Если значение не задано, то используется имя метода в котором был вызов.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? string.Empty));
        }

        /// <summary>Защищённый метод для создания события <see cref="PropertyChanged"/>.</summary>
        /// <param name="propertyName">Имя изменившегося свойства. 
        /// Если значение не задано, то используется имя метода в котором был вызов.</param>
        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args ?? allProperties);
        }
        protected static readonly PropertyChangedEventArgs allProperties = new PropertyChangedEventArgs(string.Empty);

        /// <summary>Защищённый метод для присвоения значения полю и
        /// создания события <see cref="PropertyChanged"/>.</summary>
        /// <typeparam name="T">Тип поля и присваиваемого значения.</typeparam>
        /// <param name="propertyField">Ссылка на поле.</param>
        /// <param name="newValue">Присваиваемое значение.</param>
        /// <param name="propertyName">Имя изменившегося свойства. 
        /// Если значение не задано, то используется имя метода в котором был вызов.</param>
        /// <param name="equality">Делегат для сравнения значения поля с новым значением.</param>
        /// <returns>Возвращает <see langword="true"/>, если значение изменилось и
        /// было поднято событие <see cref="PropertyChanged"/>.</returns>
        /// <remarks>Метод предназначен для использования в сеттере свойства.<br/>
        /// Сравнение нового значения со значением поля производится <see cref="object.Equals(object, object)"/>.<br/>
        /// Если присваиваемое значение не эквивалентно значению поля, то оно присваивается полю.<br/>
        /// После присвоения создаётся событие <see cref="PropertyChanged"/> вызовом
        /// метода <see cref="RaisePropertyChanged(string)"/>
        /// с передачей ему параметра <paramref name="propertyName"/>.<br/>
        /// После создания события вызывается метод <see cref="OnPropertyChanged(string, object, object)"/>.</remarks>
        protected bool Set<T>(ref T propertyField, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            bool notEquals = !Equals(propertyField, newValue);
            if (notEquals)
            {
                T oldValue = propertyField;
                propertyField = newValue;
                RaisePropertyChanged(propertyName);

                OnPropertyChanged(propertyName, oldValue, newValue);
            }

            return notEquals;
        }

        /// <summary>Защищённый виртуальный метод вызывается после присвоения значения
        /// свойству и после создания события <see cref="PropertyChanged"/>.</summary>
        /// <param name="propertyName">Имя изменившегося свойства.</param>
        /// <param name="oldValue">Старое значение свойства.</param>
        /// <param name="newValue">Новое значение свойства.</param>
        /// <remarks>Переопределяется в производных классах для реализации
        /// реакции на изменение значения свойства.<br/>
        /// Рекомендуется в переопределённом методе первым оператором вызывать базовый метод.<br/>
        /// Если в переопределённом методе не будет вызова базового, то возможно нежелательное изменение логики базового класса.</remarks>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) { }
    }
}
