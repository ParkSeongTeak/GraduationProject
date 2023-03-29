using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class PlayerController : Entity
    {
        protected Define.Charcter _myClass;
        List<Item> _items = new List<Item>(); //�������� +-�ϸ� ��ü�� ��� �����ҰŶ� List
        public List<Item> MyItems { get { return _items; } }
        public Define.Charcter MyClass { get { return _myClass; } }

        // ������ �� �� ������ Status.BeAttacked(float DMG)���� ����
        public abstract void IsAttack();
        public abstract void IsSkill();
        protected override void Dead() { }
        public void IsMove()
        {
            Vector2 vector2;
            // 

        }

        public MonsterController NearMoster()
        {
            return new MonsterController();
        }
    }
}
