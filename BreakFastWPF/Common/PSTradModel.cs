using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace BreakFastWPF.Common
{
    public class PSTradModel
    {
        [DllImport("PS3_DLL.dll", EntryPoint = "Transaction_Finish", CallingConvention = CallingConvention.StdCall)]
        static extern int Transaction_Finish();
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_Transaction_Amount", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_Transaction_Amount();
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Payout_Amount", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_Payout_Amount(int nc, int nb);
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Charge_Device_Enable", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_Charge_Device_Enable();
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Charge_Device_Disable", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_Charge_Device_Disable();
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_Transaction_status", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_Transaction_status();
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_The_Tube_of_Coin_Inventory", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_The_Tube_of_Coin_Inventory();
        [DllImport("PS3_DLL.dll", EntryPoint = "Stack_the_bill", CallingConvention = CallingConvention.StdCall)]
        static extern int Stack_the_bill(int ns);
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_Payout_Amount_Report", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_Payout_Amount_Report();
        [DllImport("PS3_DLL.dll", EntryPoint = "AutoCal_Payout_Amount", CallingConvention = CallingConvention.StdCall)]
        static extern int AutoCal_Payout_Amount(int ns, int np, int nd);
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_the_payout_amount_cal_Result", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_the_payout_amount_cal_Result();
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_The_Coin_Inventory_of_Hopper", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_The_Coin_Inventory_of_Hopper();
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_the_Status_of_RFID_Device", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_the_Status_of_RFID_Device();
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_the_RFID_Card_Number", CallingConvention = CallingConvention.StdCall)]
        static extern string Request_the_RFID_Card_Number();

        public enum TransFunc
        {
            Transaction_Finish = 2, Request_Transaction_Amount,
            Setting_Payout_Amount, Setting_Charge_Device_Enable,
            Setting_Charge_Device_Disable, Request_Transaction_status,
            Request_The_Tube_of_Coin_Inventory, Stack_the_bill,
            Request_Payout_Amount_Report, AutoCal_Payout_Amount,
            Request_the_payout_amount_cal_Result, Request_The_Coin_Inventory_of_Hopper,
            Request_the_Status_of_RFID_Device, Request_the_RFID_Card_Number
        }

        public string TransFuncStr(int idx)
        {
            return ((TransFunc)idx).ToString();
        }

        public int[] convertIntPtr(IntPtr ptr)
        {
            int arrayLength = Marshal.ReadInt32(ptr);
            int[] result = new int[arrayLength];
            Marshal.Copy(ptr, result, 0, arrayLength);
            return result;
        }

        public int setTransactionFinish()
        {
            /*
             * The command will be sent to ps3 after the deal is finish already
               (a)Input:None
               (b)Return:nSuccess : int
               (1->Success, other->ErrorCode)
               (c)Example:
               int nSuccess = Transaction_Finish();
             */
            return Transaction_Finish();
        }

        public int[] reqTransactionAmount()
        {
            /*
             * Total credit be accepted from Coin changer and bill validator
               (a)Input:None
               (b)Return: pData: int *
               pData[0]->Coin amount
               pData[1]->Stacked bill amount
               pData[2]->Escrow bill amount
               pData[3]-> Escrow status
               Escrow status: 0 - no bill, 1 - bill in escrow, 2 - stacked finish, 3 - stacked fail
               (c)Example:
               int * Amount = Request_Transaction_Amount();
             */
            try
            {
                IntPtr ptr = Request_Transaction_Amount();
                int[] result = convertIntPtr(ptr);
                return result;
            }
            catch
            {

            }
            return null;
        }

        public int setPayoutAmount()
        {
            /*
             * Credit be payout to customer from Coin changer and note dispenser
               (a)Input:
               1.nPayoutCoin: int
               2.nPayoutBill: int
               (b)Return : nSuccess : int
               (1->Success, other->ErrorCode)
               (c)Example: int nSuccess =
               Setting_Payout_Amount(1, 1);
             */
            return Setting_Payout_Amount(1, 1);
        }

        public int setChargeDeviceEnable()
        {
            /*
             * Enable the device that can accept coin or bill, It depends on CMD(A7h、A9h) to enable the bill and coin.
               (a)Input:None
               (b)Return: nSuccess : int
               (1->Success, other->ErrorCode)
               (c)Example:
               int nSuccess =
               Setting_Charge_Device_Enable();
             */
            return Setting_Charge_Device_Enable();
        }

        public int setChargeDeviceDisable()
        {
            /*
             * It means disable the device that can accept coin or bill
               (a)Input:None
               (b)Return:nSuccess : int
               (1->Success, other->ErrorCode)
               (c)Example:
               int nSuccess = Setting_Charge_Device_Disable();
             */
            return Setting_Charge_Device_Disable();
        }

        public int[] reqTransactionStatus()
        {
            /*
             * It means request the transaction status from ps3. It accurately knows ps3 status
               (a)Input: None
               (b)Return:pData: int *
               pData[0]->Transaction status(Appendix-1)
               pData[1]->Device error status(Appendix-2)
               (c)Example:int * pGetData = Request_Transaction_status();
             */
            try
            {
                IntPtr ptr = Request_Transaction_status();
                int[] result = convertIntPtr(ptr);
                return result;
            }
            catch
            {

            }
            return null;
        }

        public int[] reqTheTubeofCoinInventory()
        {
            /*
             * It request the tube of coin inventory from ps3.
               (a)Input: None
               (b)Return : pData: int *
               pData[0]~ pData[15]->The tube of coin
               (c)Example:
               int * pTubeCoin = Request_The_Tube_of_Coin_Inventory();
              */
            try
            {
                IntPtr ptr = Request_The_Tube_of_Coin_Inventory();
                int[] result = convertIntPtr(ptr);
                return result;
            }
            catch
            {

            }
            return null;
        }

        public int StackTheBill(int ns)
        {
            /*
             * It stack the bill to the cashbox or return from
               the escrow position
               (a)Input:
               nStack : int
               (b)Return: nSuccess : int
               (1->Success, other->ErrorCode)
               (c)Example:
               int nSuccess = Stack The Bill(1);
             */
            return Stack_the_bill(ns);
        }

        public int[] reqPayoutAmountReport()
        {
            /*
             * It means the total credit be accepted from Coin changer and bill validator. 
             * Controller should display the credit on monitor.
               (a)Input:None
               (b)Return: pData: int *
               pData[0]->Coin payout amount report
               pData[1]->Bill payout amount report
               (c)Example:
               int * pGetData = Request_Payout_Amount_Report();
             */
            try
            {
                IntPtr ptr = Request_Payout_Amount_Report();
                int[] result = convertIntPtr(ptr);
                return result;
            }
            catch
            {

            }
            return null;
        }

        public int AutoCalPayoutAmount(int ns, int np, int nd)
        {
            /*
             * It means the rest of credit be payout to customer or Checks the Inventory that is enough to payout or not.
               (a)Input:
               1. nSetFunction: int
                    1->Start Payout
                    2->Check the Inventory amount
               2. nPayoutAmount: int
               3. nDecPoint: int
               (b)Return: nSuccess : int
               (1->Success, other->ErrorCode)
               The setting function is 0x02, ps3 will calculate the inventory is enough to payout or not. 
               Controller can order the command Request_the_payout_amount_cal_Result to know the result. 
               If setting function is 0x01, it means start to payout the rest of credit.
             */
            return AutoCal_Payout_Amount(ns, np, nd);
        }

        public int[] reqThePayoutAmountCalResult()
        {
            /*
             * It requests the payout amount calculation result from ps3.
                (a)Input:None
                (b)Return: pData: int *
                pData[0]->nResultParameter
                pData[1]->nPayoutAmount
                pData[2]->nDecimalPoint
                nResultParameter
                1->Calculate Busy
                2->Calculate Finish(Enough to payout)
                3->Calculate Finish(Not enough to payout)
                If ps3 responds to 0x02. It means the inventory is enough to payout. 
                When the ps3 responds to 0x03.it means the inventory is not enough to payout. 
                It shows the maximum value that the ps3 can pay.
              */
            try
            {
                IntPtr ptr = Request_the_payout_amount_cal_Result();
                int[] result = convertIntPtr(ptr);
                return result;
            }
            catch
            {

            }
            return null;
        }

        public int[] reqTheCoinInventoryOfHopper()
        {
            /*
             * It request the coin inventory of Hopper from ps3.
               (a)Input:None
               (b)Return: pData: int *
               pData[0]->the coin inventory of Hopper 1
               pData[1]->the coin inventory of Hopper 2
               pData[2]->the coin inventory of Hopper 3
               pData[3]->the coin inventory of Hopper 4
               The master can know how much money can be refunded. 
               The information can help master to decide to stack the bill.
             */
            try
            {
                IntPtr ptr = Request_The_Coin_Inventory_of_Hopper();
                int[] result = convertIntPtr(ptr);
                return result;
            }
            catch
            {

            }
            return null;
        }

        public int[] reqTheStatusOfRFIDDevice()
        {
            /*
             * It means the status of RFID device.
               (a)Input:None
               (b)Return: pData: int *
               pData[0]->RFID Status
               pData[1]->RFID Button
               RFID Status  RFID Button     Code means
               -----------+--------------+------------------------------
                    00H         XXH         RFID Idle
                    01H         XXH         Card Detect
                    02H         XXH         Card Change
                    03H         00H~03H     00H->Button1~ 03H->Button4
                    04H         XXH         RFID Disable
                    05H         XXH         RFID Module Failure Error
                    06H         XXH         RFID LCD Error
                    07H         XXH         RFID RTC Error
                    08H         XXH         RFID Flash Memory Error
                    09H         XXH         RFID Fram Error
                    0AH         XXH         RFID No Install
               --------------------------------------------------------
               XXH- Don’t care
               */
            try
            {
                IntPtr ptr = Request_the_Status_of_RFID_Device();
                int[] result = convertIntPtr(ptr);
                return result;
            }
            catch
            {

            }
            return null;
        }

        public string reqTheRFIDCardNumber()
        {
            /*
             * It means the inserted card’s number.
               (a)Input:None
               (b)Return: strNumber: AnsiString
             */
            return Request_the_RFID_Card_Number();
        }

    }
}
