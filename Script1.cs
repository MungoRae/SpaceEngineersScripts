// A Phase is getting the state of the elevator from an unknown state to a known state.
enum Action {
    SecureBottom,
    SecureTop,
    ReleaseTop,
    ReleaseBottom,
    ExtendPlatforms,
    RetractPlatforms,
}

enum MainPistonState {
    Retracted,
    Extended,
    Inbetween
}

class ElevatorState {
    bool bottomLock;
    bool topLock;
    MainPistonState mainPistonState;
}

public Program()
{
    // The constructor, called only once every session and
    // always before any other method is called. Use it to
    // initialize your script. 
    //     
    // The constructor is optional and can be removed if not
    // needed.
    // 
    // It's recommended to set RuntimeInfo.UpdateFrequency 
    // here, which will allow your script to run itself without a 
    // timer block.
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
}

public void Main(string argument, UpdateType updateSource)
{
    IMyTextPanel display00 = GetDisplayAtPosition(0, 0);  
    IMyTextPanel display01 = GetDisplayAtPosition(0, 1); 
    IMyTextPanel display10 = GetDisplayAtPosition(1, 0);  
    IMyTextPanel display11 = GetDisplayAtPosition(1, 1);  
    IMyTextPanel display20 = GetDisplayAtPosition(2, 0);
    IMyTextPanel display21 = GetDisplayAtPosition(2, 1);

    ClearDisplay(display00);
    ClearDisplay(display01);
    ClearDisplay(display10);
    ClearDisplay(display11);
    ClearDisplay(display20);
    ClearDisplay(display21);
    
    IMyBlockGroup lowerConnectors = LowerConnectors();
    IMyBlockGroup upperConnectors = UpperConnectors();

    IMyBlockGroup lowerPistions = LowerConnectorPistons();
    IMyBlockGroup upperPistons = UpperConnectorPistons();

    IMyBlockGroup elevationPistions = MainShaftPistons();

    PrintConnectorStatus(upperConnectors, display00);
    PrintConnectorStatus(lowerConnectors, display00);
    PrintPistonStatus(upperPistons, display01);
    PrintPistonStatus(lowerPistions, display01);
    PrintPistonStatus(elevationPistions, display11);

    ElevatorState state = GetCurrentState();
    CalculateAction(state);
}

private Elevator GetCurrentState() {
    return new ElevatorState(
        AreConnectorsSecure(UpperConnectors()),
        AreConnectorsSecure(LowerConnectors()),
        GetMainPistonState()
    );
}

/*
 * What should the elevator do given the current state of the machine?
 *
 * 
 */
private void CalculateAction(ElevatorState state) {
    if (IsInvalidState(state)) {
        throw new IllegalStateException("Elevator state is not correct. Please investigate at once!");
    }
}

private bool IsInvalidState(ElevatorState state) {
    if (state.bottomLock == true && state.topLock == true && state.mainPistonState == MainPistonState.Inbetween) 
    {
        return true;
    }
    else if(state.bottomLock == false && state.topLock == false)
    {
        return true;
    }

    return false;
}

private bool IsNextActionSecureTop(ElevatorState state) {
    return state.bottomLock == true && state.topLock == false && state.mainPistonState == MainPistonState.Extended;
}


/*
 * ================================================================================================
 * EXAMINE BLOCK STATE
 * ================================================================================================
 */


private bool AreConnectorsSecure(IMyBlockGroup connectorBlockGroup) {
    bool connectorStatus = false;
    foreach (IMyTerminalBlock block in BlocksFromGroup(connectorBlockGroup)) {
        var connector = (IMyShipConnector) block;
        if (connector.Status == MyShipConnectorStatus.Connected) {
            connectorStatus = true;
        }
        else
        {
            return connectorStatus = false;
        }
    }

    return connectorStatus;
}

private MainPistonState GetMainPistonState() {
    var pistonGroup = MainShaftPistons();
    var state = MainPistonState.Inbetween;
    /*
     * Check all blocks are 10.0f or all blocks are 0.0f. 
     * If not then return Inbetween state.
     */
    foreach (IMyTerminalBlock piston in BlocksFromGroup(pistonGroup)) {
        if (state == null) {
            if (piston.CurrentPosition == 10.0f) {
                state = MainPistonState.Extended;
            }
            else if (piston.CurrentPosition == 0.0f) {
                state = MainPistonState.Retracted;
            }
            else {
                return MainPistonState.Inbetween;
            }
        }
        else {
            if (piston.CurrentPosition != 10.0f && state == MainPistonState.Extended) {
                return MainPistonState.Inbetween;
            }
            else if (piston.CurrentPosition != 0.0f && state == MainPistonState.Retracted) {
                return MainPistonState.Inbetween; 
            }
        }
    }

    return state;
}

private bool AreAllProjectorBlocksBuilt() {
    return true;
}

private bool IsMainPistionExtended() {
    return true;
}

/*
 * ================================================================================================
 * PRINT TO DISPLAY
 * ================================================================================================
 */

private void PrintPistonStatus(IMyBlockGroup pistonBlockGroup, IMyTextPanel display) {
    foreach (IMyTerminalBlock block in BlocksFromGroup(pistonBlockGroup)) {
        var piston = (IMyPistonBase) block;
        display.WritePublicText(piston.DisplayNameText + "\n", true);
        display.WritePublicText(piston.Status + " - " + piston.CurrentPosition + "\n", true);
        display.WritePublicText("\n", true);
    }
}

private void PrintConnectorStatus(IMyBlockGroup connectorBlockGroup, IMyTextPanel display) {
    foreach (IMyTerminalBlock block in BlocksFromGroup(connectorBlockGroup)) {
        var connector = (IMyShipConnector) block;
        display.WritePublicText(connector.DisplayNameText + "\n", true);
        display.WritePublicText(connector.Status + "\n", true);
        display.WritePublicText("\n", true);
    }
}

private void WriteBlocksInSystem(IMyTextPanel display) {
    List<IMyBlockGroup> space_tether_block_groups = new List<IMyBlockGroup>();
    GridTerminalSystem.GetBlockGroups(space_tether_block_groups);   
    display.WritePublicText("Blocks in system with SL1 Prefix :>> " + space_tether_block_groups.Count + "\n", false);
    display.WritePublicText("Blocks in block groups :>> " + space_tether_block_groups.Count + "\n", false);
    for (int i = 0;  i < space_tether_block_groups.Count; i++)
    {
        display.WritePublicText("\n" + space_tether_block_groups[i].ToString(), true);
    }
}

private void ClearDisplay(IMyTextPanel display) {
    display.WritePublicText("");
}

/*
 * ================================================================================================
 * GET BLOCKS
 * ================================================================================================
 */

private IMyTextPanel GetDisplayAtPosition(int x, int y) {
    return GridTerminalSystem.GetBlockWithName("SL1 TP - " + x + " " + y) as IMyTextPanel;
}

private IMyBlockGroup LowerConnectors() {
    return GridTerminalSystem.GetBlockGroupWithName("SL1 Connectors Bottom");
}

private IMyBlockGroup UpperConnectors() {
    return GridTerminalSystem.GetBlockGroupWithName("SL1 Connectors Top");
}

private IMyBlockGroup LowerConnectorPistons() {
    return GridTerminalSystem.GetBlockGroupWithName("SL1 Pistons Bottom");
}

private IMyBlockGroup UpperConnectorPistons() {
    return GridTerminalSystem.GetBlockGroupWithName("SL1 Pistons Top");
}

private IMyBlockGroup MainShaftPistons() {
    return GridTerminalSystem.GetBlockGroupWithName("SL1 Main Pistons");
}

/*
 * ================================================================================================
 * UTILITY
 * ================================================================================================
 */

private List<IMyTerminalBlock> BlocksFromGroup(IMyBlockGroup group) {
    List<IMyTerminalBlock> blockList = new List<IMyTerminalBlock>();
    group.GetBlocks(blockList);
    return blockList;
}