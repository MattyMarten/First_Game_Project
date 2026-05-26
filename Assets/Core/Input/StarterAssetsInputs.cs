using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {

        // =========================
        // Player Movement Callbacks
        // =========================

        [Header("Player Movement Input")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool crouch;

        // =========================
        // Player Gameplay Callbacks
        // =========================

        [Header("Player Gameplay Actions")]
        public bool interact;
        public bool openInventory;
        public float hotbarScroll;

        public bool slot1;
        public bool slot2;
        public bool slot3;
        public bool slot4;
        public bool slot5;
        public bool slot6;
        public bool slot7;
        public bool slot8;
        public bool slot9;
        public bool slot0;

        public bool openMap;
        public bool openKeyItems;
        public bool openQuests;
        public bool skipMessage;

        // =========================
        // Inventory Callbacks
        // =========================

        [Header("Inventory Actions")]
        public bool closeInventory;
        public bool rightPage;
        public bool leftPage;
        public bool rotateItem;
        public bool dropHeldItem;

        // =========================
        // Debug Callbacks
        // =========================

        [Header("Debug Actions")]
        public bool debugInventory;
        public bool randomLoot;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Header("UI State")]
        public bool uiBlocked;

        public Vector2 MousePosition
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
#else
                return Input.mousePosition;
#endif
            }
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            if (uiBlocked) { move = Vector2.zero; return; }
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (uiBlocked) { look = Vector2.zero; return; }

            if (cursorInputForLook)
                LookInput(value.Get<Vector2>());
        }

        public void OnJump(InputValue value)
        {
            if (uiBlocked) { jump = false; return; }
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            if (uiBlocked) { sprint = false; return; }
            SprintInput(value.isPressed);
        }

        public void OnCrouch(InputValue value)
        {
            if (uiBlocked) { crouch = false; return; }
            CrouchInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            if (uiBlocked) return;
            if (value.isPressed) interact = true;
        }

        public void OnOpenInventory(InputValue value)
        {
            if (value.isPressed)
                openInventory = true;
        }

        public void OnHotbarScroll(InputValue value)
        {
            hotbarScroll = value.Get<float>();
        }

        public void OnSlot1(InputValue value) { if (value.isPressed) slot1 = true; }
        public void OnSlot2(InputValue value) { if (value.isPressed) slot2 = true; }
        public void OnSlot3(InputValue value) { if (value.isPressed) slot3 = true; }
        public void OnSlot4(InputValue value) { if (value.isPressed) slot4 = true; }
        public void OnSlot5(InputValue value) { if (value.isPressed) slot5 = true; }
        public void OnSlot6(InputValue value) { if (value.isPressed) slot6 = true; }
        public void OnSlot7(InputValue value) { if (value.isPressed) slot7 = true; }
        public void OnSlot8(InputValue value) { if (value.isPressed) slot8 = true; }
        public void OnSlot9(InputValue value) { if (value.isPressed) slot9 = true; }
        public void OnSlot0(InputValue value) { if (value.isPressed) slot0 = true; }

        public void OnOpenMap(InputValue value) { if (value.isPressed) openMap = true; }
        public void OnOpenKeyItems(InputValue value) { if (value.isPressed) openKeyItems = true; }
        public void OnOpenQuests(InputValue value) { if (value.isPressed) openQuests = true; }
        public void OnSkipMessage(InputValue value) { if (value.isPressed) skipMessage = true; }

        public void OnCloseInventory(InputValue value) { if (value.isPressed) closeInventory = true; }
        public void OnRightPage(InputValue value) { if (value.isPressed) rightPage = true; }
        public void OnLeftPage(InputValue value) { if (value.isPressed) leftPage = true; }
        public void OnRotateItem(InputValue value) { if (value.isPressed) rotateItem = true; }
        public void OnDropHeldItem(InputValue value) { if (value.isPressed) dropHeldItem = true; }

        public void OnDebugInventory(InputValue value) { if (value.isPressed) debugInventory = true; }
        public void OnRandomLoot(InputValue value) { if (value.isPressed) randomLoot = true; }
#endif

        public void MoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
        public void LookInput(Vector2 newLookDirection) => look = newLookDirection;
        public void JumpInput(bool newJumpState) => jump = newJumpState;
        public void SprintInput(bool newSprintState) => sprint = newSprintState;
        public void CrouchInput(bool newCrouchState) => crouch = newCrouchState;

        public bool ConsumeInteract() => Consume(ref interact);
        public bool ConsumeOpenInventory() => Consume(ref openInventory);
        public bool ConsumeCloseInventory() => Consume(ref closeInventory);
        public bool ConsumeRightPage() => Consume(ref rightPage);
        public bool ConsumeLeftPage() => Consume(ref leftPage);
        public bool ConsumeRotateItem() => Consume(ref rotateItem);
        public bool ConsumeDropHeldItem() => Consume(ref dropHeldItem);
        public bool ConsumeOpenMap() => Consume(ref openMap);
        public bool ConsumeOpenKeyItems() => Consume(ref openKeyItems);
        public bool ConsumeOpenQuests() => Consume(ref openQuests);
        public bool ConsumeSkipMessage() => Consume(ref skipMessage);
        public bool ConsumeDebugInventory() => Consume(ref debugInventory);
        public bool ConsumeRandomLoot() => Consume(ref randomLoot);

        private bool Consume(ref bool value)
        {
            if (!value) return false;
            value = false;
            return true;
        }

        public void ClearOneFrameInputs()
        {
            ClearPlayerOneFrameInputs();
            ClearInventoryOneFrameInputs();
            ClearDebugOneFrameInputs();
        }

        private void ClearPlayerOneFrameInputs()
        {
            interact = false;
            openInventory = false;
            openMap = false;
            openKeyItems = false;
            openQuests = false;
            skipMessage = false;

            slot0 = false;
            slot1 = false;
            slot2 = false;
            slot3 = false;
            slot4 = false;
            slot5 = false;
            slot6 = false;
            slot7 = false;
            slot8 = false;
            slot9 = false;

            hotbarScroll = 0f;
        }

        private void ClearInventoryOneFrameInputs()
        {
            closeInventory = false;
            rightPage = false;
            leftPage = false;
            rotateItem = false;
            dropHeldItem = false;
        }

        private void ClearDebugOneFrameInputs()
        {
            debugInventory = false;
            randomLoot = false;
        }

        private void LateUpdate()
        {
            ClearOneFrameInputs();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        public void SetCursorState(bool newState)
        {
            cursorLocked = newState;
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}