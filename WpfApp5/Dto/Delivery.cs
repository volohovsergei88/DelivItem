namespace Dto
{
    /// <summary>Dto сущность. Подразумевается,
    /// что нет задачи динамического изменения свойств и,
    /// соответсвенно, отслеживания изменений их значений. </summary>
    public struct Delivery
    {
        public Delivery(int id, string namedelivery, int parentId = 0)
        {
            Id = id;
            NameDelivery = namedelivery;
            ParentId = parentId;
        }


        /// <summary>Идентификатор сущности.</summary>
        public int Id { get; set; }

        public string NameDelivery { get; set; }

        /// <summary>Id родительской сущности. Если 0, то это сущность верхнего уровня.</summary>
        public int ParentId { get; set; }
    }

}
