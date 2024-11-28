using System;

namespace GOBA.CORE
{
    [Flags]
    public enum AbilityBehaviour
    {
        DOTA_ABILITY_BEHAVIOR_NO_TARGET = 1 << 0,  // Doesn't need a target to be cast. Ability fires off as soon as the button is pressed.
        DOTA_ABILITY_BEHAVIOR_UNIT_TARGET = 1 << 1,  // Needs a target to be cast on. Requires AbilityUnitTargetTeam and AbilityUnitTargetType, see Targeting.
        DOTA_ABILITY_BEHAVIOR_POINT = 1 << 2,  // Can be cast anywhere the mouse cursor is. If a unit is clicked, it will just be cast where the unit was standing.
        DOTA_ABILITY_BEHAVIOR_PASSIVE = 1 << 3,  // Cannot be cast.
        DOTA_ABILITY_BEHAVIOR_CHANNELLED = 1 << 4,  // Channeled ability. If the user moves, or is silenced/stunned, the ability is interrupted.
        DOTA_ABILITY_BEHAVIOR_TOGGLE = 1 << 5,  // Can be toggled On/Off.
        DOTA_ABILITY_BEHAVIOR_AURA = 1 << 6,  // Ability is an aura. Not really used other than to tag the ability as such.
        DOTA_ABILITY_BEHAVIOR_AUTOCAST = 1 << 7,  // Can be cast automatically. Usually doesn't work by itself in anything that is not an ATTACK ability.
        DOTA_ABILITY_BEHAVIOR_HIDDEN = 1 << 8,  // Can't be cast, and won't show up on the HUD.
        DOTA_ABILITY_BEHAVIOR_AOE = 1 << 9,  // Can draw a radius where the ability will have effect. Like POINT, but with an area of effect display. Makes use of AOERadius.
        DOTA_ABILITY_BEHAVIOR_NOT_LEARNABLE = 1 << 10, // Cannot be learned by clicking on the HUD. Example: Invoker's abilities.
        DOTA_ABILITY_BEHAVIOR_ITEM = 1 << 11, // Ability is tied to an item. There is no need to use this, the game will internally assign this behavior to any "item_datadriven".
        DOTA_ABILITY_BEHAVIOR_DIRECTIONAL = 1 << 12, // Has a direction from the hero. Examples: Mirana's Arrow, or Pudge's Hook.
        DOTA_ABILITY_BEHAVIOR_IMMEDIATE = 1 << 13, // Can be used instantly, without going into the action queue.
        DOTA_ABILITY_BEHAVIOR_NOASSIST = 1 << 14, // Ability has no reticle assist. (?)
        DOTA_ABILITY_BEHAVIOR_ATTACK = 1 << 15, // Is an attack, and cannot hit attack-immune targets.
        DOTA_ABILITY_BEHAVIOR_ROOT_DISABLES = 1 << 16, // Cannot be used when rooted.
        DOTA_ABILITY_BEHAVIOR_UNRESTRICTED = 1 << 17, // Ability is allowed when commands are restricted. Example: Lifestealer's Consume.
        DOTA_ABILITY_BEHAVIOR_DONT_ALERT_TARGET = 1 << 18, // Does not alert enemies when target-cast on them. Example: Spirit Breaker's Charge.
        DOTA_ABILITY_BEHAVIOR_DONT_RESUME_MOVEMENT = 1 << 19, // Should not resume movement when it completes. Only applicable to no-target, non-immediate abilities.
        DOTA_ABILITY_BEHAVIOR_DONT_RESUME_ATTACK = 1 << 20, // Ability should not resume command-attacking the previous target when it completes. Only applicable to no-target, non-immediate abilities and unit-target abilities.
        DOTA_ABILITY_BEHAVIOR_NORMAL_WHEN_STOLEN = 1 << 21, // Ability still uses its normal cast point when stolen. Examples: Meepo's Poof, Furion's Teleport.
        DOTA_ABILITY_BEHAVIOR_IGNORE_BACKSWING = 1 << 22, // Ability ignores backswing pseudoqueue.
        DOTA_ABILITY_BEHAVIOR_IGNORE_PSEUDO_QUEUE = 1 << 23, // Can be executed while stunned, casting, or force-attacking. Only applicable to toggled abilities. Example: Morphling's Attribute Shift.
        DOTA_ABILITY_BEHAVIOR_RUNE_TARGET = 1 << 24, // Targets runes.
        DOTA_ABILITY_BEHAVIOR_IGNORE_CHANNEL = 1 << 25, // Doesn't cancel abilities with _CHANNELED behavior.
        DOTA_ABILITY_BEHAVIOR_OPTIONAL_UNIT_TARGET = 1 << 26, // Bottle and Wards.
        DOTA_ABILITY_BEHAVIOR_OPTIONAL_NO_TARGET = 1 << 27, // (?)
    }
}