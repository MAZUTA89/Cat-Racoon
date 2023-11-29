using ClientServer.Client;
using ClientServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Lobby.LoadingLevel
{
    public class LevelSignalChecker
    {
        public async Task<bool> CheckServerLevelSignal(Server server)
        {
            LoadLevelSignal loadLevelSignal
                = new LoadLevelSignal(ConnectionType.Server);

            await server.SendAcync(loadLevelSignal);

            LoadLevelSignal recvLevelSignal;

            try
            {
                recvLevelSignal = await server.RecvAcync<LoadLevelSignal>();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return false;
            }

            if (recvLevelSignal.ConnectionType == ConnectionType.Client)
                return true;
            else
                return false;
        }

        public async Task<bool> CheckClientLevelSignal(Client client)
        {
            LoadLevelSignal loadLevelSignal 
                = new LoadLevelSignal(ConnectionType.Client);

            await client.SendAcync(loadLevelSignal);

            LoadLevelSignal recvLevelSignal;
            try
            {
                recvLevelSignal = await client.RecvAcync<LoadLevelSignal>();
            }catch (Exception ex)
            {
                Debug.Log(ex);
                return false;
            }

            if (recvLevelSignal.ConnectionType == ConnectionType.Server)
                return true;
            else
                return false;
        }
    }
}
