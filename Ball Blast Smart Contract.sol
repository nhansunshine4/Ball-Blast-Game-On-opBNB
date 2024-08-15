// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;

contract BallBlastGame {
    mapping(address => uint256) private txCount;
    mapping(address => uint256) private highestLevel;

    // Events
    event txCountEvent(address indexed player, uint256 newCount);
    event highestLevelEvent(address indexed player, uint256 newSaving);

    function getTxCount(address player) external view returns (uint256) {
        return txCount[player];
    }

    function setTxCount(address player) external {
        txCount[player] += 1;
        emit txCountEvent(player, txCount[player]);
    }

    function getHighestLevel(address player) external view returns (uint256) {
        return highestLevel[player];
    }

    function setHighestLevel(address player, uint256 savingLevel) external {
        highestLevel[player] = savingLevel;
        emit highestLevelEvent(player, savingLevel);
    }
}
