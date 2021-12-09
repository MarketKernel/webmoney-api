using System;

namespace WebMoney.XmlInterfaces.BasicObjects
{
    [Serializable]
    public enum WmCurrency
    {
        None = 0,
        Z = 1, // Доллар
        E, // Евро
        R, // Рубль
        U, // Гривна
        B, // Рубль РБ
        G, // Золото
        D, // Мне должны
        C, // Я должен
        X, // Bitcoin
        H, // Bitcoin Cash (BCH)
        L, // Litecoin
        K, // Казахский тенге
        V, // Вьетнамский донг
        
        // Отключено.
        Y = 100, // Узбекский сум

        // Не используется!
        A = 200,
        F,
        I,
        J,
        M,
        N,
        O,
        P,
        Q,
        S,
        T,
        W
    }
}