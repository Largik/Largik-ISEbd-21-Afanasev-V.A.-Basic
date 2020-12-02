﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ship
{
    /// <summary>
    /// Параметризованный класс для хранения набора объектов от интерфейса ITransport
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Port<T> where T : class, ITransport
    {
        /// <summary>
        /// Список объектов, которые храним
        /// </summary>
        private readonly List<T> _places;
        /// <summary>
        /// Максимальное количество мест в порту
        /// </summary>
        private readonly int _maxCount;
        /// <summary>
        /// Ширина окна отрисовки
        /// </summary>
        private readonly int _pictureWidth;
        /// <summary>
        /// Высота окна отрисовки
        /// </summary>
        private readonly int _pictureHeight;
        /// <summary>
        /// Размер парковочного места (ширина)
        /// </summary>
        private readonly int _placeSizeWidth = 210;
        /// <summary>
        /// Размер парковочного места (высота)
        /// </summary>
        private readonly int _placeSizeHeight = 120;
        /// <summary>
        /// Свободные места
        /// </summary>
        private readonly int _column;
        /// <summary>
        /// Координата X для следующего
        /// </summary>
        public int XShip { get; private set; } = 5;
        /// <summary>
        /// Координата Y для следующего
        /// </summary>
        public int YShip { get; private set; } = 50;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="picWidth">Размер порта - ширина</param>
        /// <param name="picHeight">Размер порта - высота</param>
        public Port(int picWidth, int picHeight)
        {
            int width = picWidth / _placeSizeWidth;
            int height = picHeight / _placeSizeHeight;
            _maxCount = width * height;
            _column = height;
            _pictureWidth = picWidth;
            _pictureHeight = picHeight;
            _places = new List<T>();
        }
        /// <summary>
        /// Перегрузка оператора сложения
        /// Логика действия: на парковку добавляется корабль
        /// </summary>
        /// <param name="p">Парковка</param>
        /// <param name="ship">Добавляемый корабль</param>
        /// <returns></returns>
        public static bool operator +(Port<T> Port, T ship)
        {
            if (Port._places.Count >= Port._maxCount)
            {
                throw new PortOverflowException();
            }
            Port._places.Add(ship);
            return true;
        }
        /// <summary>
        /// Перегрузка оператора вычитания
        /// Логика действия: с парковки забираем корабль
        /// </summary>
        /// <param name="p">Порт</param>
        /// <param name="index">Индекс места, с которого пытаемся извлечь объект</param>
        /// <returns></returns>
        public static T operator -(Port<T> Port, int index)
        {
            if (index < -1 || index > Port._places.Count)
            {
                throw new PortNotFoundException(index);
            }
            T ship = Port._places[index];
            Port._places.RemoveAt(index);
            return ship;
        }
        /// <summary>
        /// Метод отрисовки порта
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            DrawMarking(g);
            for (int i = 0; i < _places.Count; ++i)
            {
                _places[i].SetPosition(i / _column * _placeSizeWidth + 5, 50 + i % _column * _placeSizeHeight,
                    _pictureWidth, _pictureHeight);
                _places[i].DrawTransport(g);
            }
        }
        /// <summary>
        /// Метод отрисовки разметки парковочных мест
        /// </summary>
        /// <param name="g"></param>
        private void DrawMarking(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 3);
            for (int i = 0; i < _pictureWidth / _placeSizeWidth; i++)
            {
                for (int j = 0; j < _pictureHeight / _placeSizeHeight + 1; ++j)
                {//линия разметки места
                    g.DrawLine(pen, i * _placeSizeWidth, j * _placeSizeHeight, i *
                   _placeSizeWidth + _placeSizeWidth / 2, j * _placeSizeHeight);
                }
                g.DrawLine(pen, i * _placeSizeWidth, 0, i * _placeSizeWidth,
               (_pictureHeight / _placeSizeHeight) * _placeSizeHeight);
            }
        }
        /// <summary>
        /// Функция получения элементы из списка
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetNext(int index)
        {
            if (index < 0 || index >= _places.Count)
            {
                return null;
            }
            return _places[index];
        }
    }
}