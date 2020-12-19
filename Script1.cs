enum States {

    Building,
        // phase: extend platform
    Moving,
        // phase: secure top
        // phase: release bottom
        // phase: contract platforms
}


// A Phase is getting the state of the elevator from an unknown state to a known state.
enum Phase {
    SecureBottom,
    SecureTop,
    ReleaseTop,
    ReleaseBottom,
    ExtendPlatforms,
    ContractPlatforms,
}

interface Action {
    public List<Phase> Phases();

    public Phase CurrentPhase();
}

class MoveUp : Action {
    private Phase currentPhase = null;
    public List<Phase> Phases() {
        return new List<Phase>() {
            Phase.SecureBottom,
            Phase.ReleaseTop,
            Phase.ExtendPlatforms,
            Phase.SecureTop,
            Phase.ReleaseBottom,
            Phase.ContractPlatforms,
            Phase.SecureBottom
        };
    }

    public Phase currentPhase() {
        return currentPhase;
    }
}

State currentState = null;

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

    var phases = currentAction.Phases();

    // if (AreConnectorsSecure(upperConnectors) && AreConnectorsSecure(lowerConnectors) && O() && ) {
    //     if (AreAllProjectorBlocksBuilt()) {
    //         // transition to move phase
    //     } else {
    //         // still building
    //     }
    // }
    // if (AreConnectorsSecure(lowerConnectors) && !AreConnectorsSecure(upperConnectors)) {
    //     if (AreAllProjectorBlocksBuilt()) {
    //         // transition to move phase
    //     } else {
    //         // you are in the build phase
    //     }
    // }

    // if (!AreConnectorsSecure(lowerConnectors) && AreConnectorsSecure(upperConnectors)) {
    //     // should be moving
    //     // else throw wobbly
    //     if (AreAllProjectorBlocksBuilt()) {
    //         // should be moving
    //     } else {
    //         // error condition
    //     }
    // }

    if (!MainPistonFullyExtended()) {
        if (!AreConnectorsSecure(lowerConnectors)) {
            SecureBottom();
            // extend lower pistons
            // secure connections
            // if (IsMainPistionExtended()) {
            //     // extend upper platform pistons
            //     // secure connections
            // } else {
            //     // extend main pistons
            //     // extend upper platform pistons
            //     // secure connections
            // }
            return;
        }
        else if ()
        }
    }
    
}

private void SecureBottom() {
    if (!PistonAtMaxExtension()) {
        ExtendLowerPistons();
        return;
    } 
    else if(LandingGearLocked()) {
        UnlockLandingGear();
        return;
    }
    else {
        LockBottomConnectors();
        return;
    }
}

private void ExtendLowerPistons() {
    IMyBlockGroup lowerPistions = LowerConnectorPistons();
    List<IMyTerminalBlock> blockList = new List<IMyTerminalBlock>();
    pistonBlockGroup.GetBlocks(blockList);
}

private bool IsReadyToBuild() {
    return AreConnectorsSecure(lowerConnectors) && !AreConnectorsSecure(UpperConnectors) && !AreAllProjectorBlocksBuilt() && !IsMainPistionExtended();
}

private bool IsReadyToMove() {
    return AreConnectorsSecure(lowerConnectors) && AreConnectorsSecure(UpperConnectors) && AreAllProjectorBlocksBuilt() && IsMainPistionExtended(); 
}

private void PrintPistonStatus(IMyBlockGroup pistonBlockGroup, IMyTextPanel display) {
    List<IMyTerminalBlock> blockList = new List<IMyTerminalBlock>();
    pistonBlockGroup.GetBlocks(blockList);
    foreach (IMyTerminalBlock block in blockList) {
        var piston = (IMyPistonBase) block;
        display.WritePublicText(piston.DisplayNameText + "\n", true);
        display.WritePublicText(piston.Status + " - " + piston.CurrentPosition + "\n", true);
        display.WritePublicText("\n", true);
    }
}

private void PrintConnectorStatus(IMyBlockGroup connectorBlockGroup, IMyTextPanel display) {
    List<IMyTerminalBlock> blockList = new List<IMyTerminalBlock>();
    connectorBlockGroup.GetBlocks(blockList);
    foreach (IMyTerminalBlock block in blockList) {
        var connector = (IMyShipConnector) block;
        display.WritePublicText(connector.DisplayNameText + "\n", true);
        display.WritePublicText(connector.Status + "\n", true);
        display.WritePublicText("\n", true);
    }
}

private bool AreConnectorsSecure(IMyBlockGroup connectorBlockGroup, MyShipConnectorStatus status) {
    List<IMyTerminalBlock> blockList = new List<IMyTerminalBlock>();
    bool connectorStatus = false;
    connectorBlockGroup.GetBlocks(blockList);
    foreach (IMyTerminalBlock block in blockList) {
        var connector = (IMyShipConnector) block;
        if (connector.Status == status) {
            connectorStatus = true;
        }
        else
        {
            return connectorStatus = false;
        }
    }

    return connectorStatus;
}

private bool AreAllProjectorBlocksBuilt() {
    return true;
}

private bool IsMainPistionExtended() {
    return true;
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

private void ClearDisplay(IMyTextPanel display) {
    display.WritePublicText("");
}