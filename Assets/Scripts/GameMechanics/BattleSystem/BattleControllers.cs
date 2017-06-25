﻿using System;

namespace HauntedCity.GameMechanics.BattleSystem
{
    public interface IShooter
    {
        void Shoot(Weapon weapon);
    }

    public interface IWeaponSpeedController
    {
        bool CanShoot(Weapon weapon);
        void BlockWeapon(Weapon weapon, float duration);
    }


    public abstract class BattleController
    {
        public event Action OnReset;
        public event Action OnDeath;
        public event Action<int> OnHealthChange;

        protected BattleStats battleStats;

        public void Kill()
        {
            battleStats.Solidity.Kill();
            if (OnDeath != null)
            {
                OnDeath();
            }
        }

        public void TakeDamage(int damage)
        {
            int startedHealth = battleStats.CurrentHealth;
            battleStats.Solidity.Attack(damage);
            int deltaHealth = startedHealth - battleStats.CurrentHealth;
            if (deltaHealth != 0)
            {
                if (OnHealthChange != null)
                {
                    OnHealthChange(-deltaHealth);
                }
            }
            if (!battleStats.Solidity.IsAlive())
            {
                if (OnDeath != null)
                {
                    OnDeath();
                }
            }
        }

        public void Heal(int healPoints)
        {
            int startedHealth = battleStats.CurrentHealth;
            battleStats.Solidity.Heal(healPoints);
            int deltaHealth = startedHealth - battleStats.CurrentHealth;
            if (deltaHealth != 0)
            {
                if (OnHealthChange != null)
                {
                    OnHealthChange(deltaHealth);
                }
            }
        }

        public virtual void Reset()
        {
            battleStats.Solidity.ResetHealth();
            if (OnReset != null)
            {
                OnReset();
            }
        }

        public virtual void Regenerate()
        {
            battleStats.Solidity.Regenerate();
        }
    }

    public class PlayerBattleController : BattleController
    {
        private IShooter _shooter;
        private IWeaponSpeedController _weaponSpeedController;
        public event Action<int> OnEnergyChanged;

        private const float _TOLERANCE = 0.0001f;

        public PlayerBattleStats BattleStats
        {
            get { return battleStats as PlayerBattleStats; }
            set { battleStats = value; }
        }

        public bool TryShoot()
        {
            if (BattleStats.CurrentEnergy < BattleStats.CurrentWeapon.ShootCost) return false;
            if (!_weaponSpeedController.CanShoot(BattleStats.CurrentWeapon)) return false;
            _shooter.Shoot(BattleStats.CurrentWeapon);
            BattleStats.CurrentEnergy -= BattleStats.CurrentWeapon.ShootCost;
            if (OnEnergyChanged != null)
            {
                OnEnergyChanged(BattleStats.CurrentWeapon.ShootCost);
            }
            if (Math.Abs(BattleStats.CurrentWeapon.Cooldown) > _TOLERANCE)
            {
                _weaponSpeedController.BlockWeapon(BattleStats.CurrentWeapon, BattleStats.CurrentWeapon.Cooldown);
            }
            return true;
        }

        public override void Reset()
        {
            BattleStats.CurrentEnergy = BattleStats.MaxEnergy;
            base.Reset();
        }

        public void RestoreEnergy(int energyPoint)
        {
            int oldEnergy = BattleStats.CurrentEnergy;
            BattleStats.CurrentEnergy += energyPoint;
            int energyDelta = BattleStats.CurrentEnergy - oldEnergy;
//            if (energyDelta != 0)
//            {
                if (OnEnergyChanged != null)
                {
                    OnEnergyChanged(energyDelta);
                }
//            }
        }
        
        public override void Regenerate()
        {
            base.Regenerate();
            RestoreEnergy(BattleStats.EnergyRegen);
        }

        public PlayerBattleController(PlayerBattleStats stats, IShooter shooter, IWeaponSpeedController speedController)
        {
            battleStats = stats;
            _shooter = shooter;
            _weaponSpeedController = speedController;
        }
    }

    public class EnemyBattleController : BattleController
    {
        public EnemyBattleStats BattleStats
        {
            get { return battleStats as EnemyBattleStats; }
        }

        public EnemyBattleController(EnemyBattleStats stats)
        {
            battleStats = stats;
        }
    }
}