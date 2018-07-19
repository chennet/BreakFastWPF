using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace BreakFast.Common
{
    public class PSClass : INotifyPropertyChanged
    {
        [DllImport("PS3_DLL.dll", EntryPoint = "Open_ComPort", CallingConvention = CallingConvention.StdCall)]
        static extern bool Open_ComPort(string strComNo, string strBaud);
        [DllImport("PS3_DLL.dll", EntryPoint = "CloseComPort", CallingConvention = CallingConvention.StdCall)]
        static extern bool CloseComPort();
        [DllImport("PS3_DLL.dll", EntryPoint = "Set_ScFa_DePl", CallingConvention = CallingConvention.StdCall)]
        static extern int Set_ScFa_DePl(int ns, int nd);
        [DllImport("PS3_DLL.dll", EntryPoint = "Get_ScFa_DePl", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Get_ScFa_DePl();
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_The_Value_of_Bill_Validator", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_The_Value_of_Bill_Validator(int[] nv);
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_The_Value_of_Bill_Validator", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_The_Value_of_Bill_Validator();
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Each_Type_of_Bill_Enable", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_Each_Type_of_Bill_Enable(int[] nv);
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_Each_Type_of_Bill_Enable", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_Each_Type_of_Bill_Enable();
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_The_Value_of_Note_Dispenser", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_The_Value_of_Note_Dispenser(int[] nv);
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_The_Value_of_Note_Dispenser", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_The_Value_of_Note_Dispenser();
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_The_Function_of_Payout_bill", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_The_Function_of_Payout_bill(int[] nv);
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_The_Function_of_Payout_bill", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_The_Function_of_Payout_bill();
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_the_number_of_bills_in_ND", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_the_number_of_bills_in_ND(int[] nv);
        [DllImport("PS3_DLL.dll", EntryPoint = "Request_the_number_of_bills_in_ND", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr Request_the_number_of_bills_in_ND();
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Coin_Device_SF_DP", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_Coin_Device_SF_DP(int ns, int nd);
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Coin_Acceptor_parameter", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_Coin_Acceptor_parameter(int[] nv);
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Hopper_parameter", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_Hopper_parameter(int[] nv);
        [DllImport("PS3_DLL.dll", EntryPoint = "Setting_the_Category_of_Hopper", CallingConvention = CallingConvention.StdCall)]
        static extern int Setting_the_Category_of_Hopper(int nv);

        public enum PSFunc
        {
            Set_ScFa_DePl = 2, Get_ScFa_DePl,
            Setting_The_Value_of_Bill_Validator, Request_The_Value_of_Bill_Validator,
            Setting_Each_Type_of_Bill_Enable, Request_Each_Type_of_Bill_Enable,
            Setting_The_Value_of_Note_Dispenser, Request_The_Value_of_Note_Dispenser,
            Setting_The_Function_of_Payout_bill, Request_The_Function_of_Payout_bill,
            Setting_the_number_of_bills_in_ND, Request_the_number_of_bills_in_ND,
            Setting_Coin_Device_SF_DP, Setting_Coin_Acceptor_parameter,
            Setting_Hopper_parameter, Setting_the_Category_of_Hopper
        }

        public string PSFuncStr(int idx)
        {
            return ((PSFunc)idx).ToString();
        }

        public int[] convertIntPtr(IntPtr ptr)
        {
            int arrayLength = Marshal.ReadInt32(ptr);
            int[] result = new int[arrayLength];
            Marshal.Copy(ptr, result, 0, arrayLength);
            return result;
        }

        public bool PSOpenPort(string ComNo, string bRate = "9600,E,8,1")
        {
            //string ComNo = "COM3";
            return Open_ComPort(ComNo, bRate);
        }

        public bool PSClosePort()
        {
            return CloseComPort();
        }

        public int initPS3()
        {
            /* Set parameters of bill */
            int rtn = setBillFunc();

            if (rtn != PSCons.PS_SUCCESS) return rtn;
            rtn = getBillFunc();
            if (rtn != PSCons.PS_SUCCESS) return rtn;

            return PSCons.PS_SUCCESS;
        }


        private int setBillFunc()
        {
            /* 
             * Set scaling factor and decimal places of bill
               (a)Input:
               1. nScaling : int
               2. nDecimal : int
               (b)Return:
               nSuccess : int
               (1->Success, other->ErrorCode)
               (c)Example:
               int nSuccess = Set_ScFa_DePl(100, 0);    <- TWD, decimal point:0
            */
            if (Set_ScFa_DePl(PSCons.bill_scale_fact, PSCons.decimal_point) != PSCons.PS_SUCCESS) return (int)PSFunc.Set_ScFa_DePl;
            /* 
             * pBill[0] = 1;鈔票面額(100)除以scaling factor(100)
               pBill[1] = 2;鈔票面額(200)除以scaling factor(200)
               pBill[2] = 5;鈔票面額(500)除以scaling factor(500)
               pBill[3] = 10;鈔票面額(1000)除以scaling factor(1000)
               …
               pBill[15] = 0;
            */
            int[] pBill = new int[16] { PSCons.pBAValue_100 / PSCons.bill_scale_fact, PSCons.pBAValue_200 / PSCons.bill_scale_fact, PSCons.pBAValue_500 / PSCons.bill_scale_fact, PSCons.pBAValue_1000 / PSCons.bill_scale_fact, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (Setting_The_Value_of_Bill_Validator(pBill) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_The_Value_of_Bill_Validator;
            /* 
             * pbEnable[0] = 1; <- Enable
               pbEnable[1] = 1;
               pbEnable[2] = 1;
               pbEnable[3] = 1;
               …
               pbEnable[15] = 0; <- Disable
             */
            // Only bill 100 enabled
            int[] pbEnable = new int[16] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (Setting_Each_Type_of_Bill_Enable(pbEnable) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_Each_Type_of_Bill_Enable;
            /* Note Dispenser 出鈔機 (ND)
             * pNDValue [0] = 1;鈔票面額(100)除以scaling factor(100)
               pNDValue [1] = 0;
             */
            int[] pNDValue = new int[2] { PSCons.pBAValue_100 / PSCons.bill_scale_fact, 0 };
            if (Setting_The_Value_of_Note_Dispenser(pNDValue) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_The_Value_of_Note_Dispenser;
            /*
             * pNDEnable[0] = 1; <- Enable
               pNDEnable[1] = 0; <- Disable
             */
            int[] pNDEnable = new int[2] { 1, 0 };
            if (Setting_The_Function_of_Payout_bill(pNDEnable) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_The_Function_of_Payout_bill;
            /*
             * pNDCount[0] = 55; <-- 鈔票面額 100 的有 55 張？
               pNDCount[1] = 0;
             */
            int[] pNDCount = new int[2] { 55, 0 };
            if (Setting_the_number_of_bills_in_ND(pNDCount) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_the_number_of_bills_in_ND;
            return PSCons.PS_SUCCESS;
        }

        private int getBillFunc()
        {
            IntPtr ptr = Get_ScFa_DePl();
            int[] pBASfDp = convertIntPtr(ptr);
            if (pBASfDp[0] != PSCons.pBASfDp_100 && pBASfDp[1] != 0) return (int)PSFunc.Get_ScFa_DePl;
            ptr = Request_The_Value_of_Bill_Validator();
            int[] pBAValue = convertIntPtr(ptr);
            return PSCons.PS_SUCCESS;
        }

        private int setCoinFunc()
        {
            /* TWD, decimal point:0*/
            if (Setting_Coin_Device_SF_DP(PSCons.coin_scale_fact, PSCons.decimal_point) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_Coin_Device_SF_DP;
            /*
             * pCAValue[0] = 1;硬幣面額(1)除以scaling factor(1)
               pCAValue[1] = 5;硬幣面額(5)除以scaling factor(1)
               pCAValue[2] = 10;硬幣面額(10)除以scaling factor(1)
               pCAValue[3] = 50;硬幣面額(50)除以scaling factor(1)
               …
               pCAValue[15] = 0;
             */
            int[] pCAValue = new int[16] { PSCons.pCAValue_1 / PSCons.coin_scale_fact, PSCons.pCAValue_5 / PSCons.coin_scale_fact, PSCons.pCAValue_10 / PSCons.coin_scale_fact, PSCons.pCAValue_50 / PSCons.coin_scale_fact, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (Setting_Coin_Acceptor_parameter(pCAValue) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_Coin_Acceptor_parameter;
            /*
             * pHopper[0] = 1;enable
               pHopper[1] = 1;
               pHopper[2] = 0;disable
               pHopper[3] = 0;
               pHopper[4] = 5;硬幣面額(5)除以scaling factor(1)
               pHopper[5] = 10;硬幣面額(10)除以scaling factor(1)
               pHopper[6] = 0;
               pHopper[7] = 0;
              */
            int[] pHopper = new int[8] { 1, 1, 0, 0, PSCons.pCAValue_5 / PSCons.coin_scale_fact, PSCons.pCAValue_10 / PSCons.coin_scale_fact, 0, 0 };
            if (Setting_Hopper_parameter(pHopper) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_Hopper_parameter;
            /*
             */
            if (Setting_the_Category_of_Hopper(1) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_the_Category_of_Hopper;
            return PSCons.PS_SUCCESS;
        }


        public static class PSCons
        {
            public const int pBASfDp_100 = 100;
            public const int pBAValue_100 = 100;
            public const int pBAValue_200 = 200;
            public const int pBAValue_500 = 500;
            public const int pBAValue_1000 = 1000;
            public const int pCAValue_1 = 1;
            public const int pCAValue_5 = 5;
            public const int pCAValue_10 = 10;
            public const int pCAValue_50 = 50;
            public const int bill_scale_fact = 100;
            public const int coin_scale_fact = 1;
            public const int decimal_point = 0;
            public const int PS_SUCCESS = 1;

        }

        public enum TransStatus
        {
            Autocheck, WaitTheDealStart, ProcessTheDeal, WaitPayout,
            PayoutBusy, PayoutFinish, PayoutFailed, RefundButtonWasDetected
        }
        public enum DeviceErrStatus
        {
            NoDeviceError, AnErrorOccurredInTheDeviced
        }
        public enum CoinChanger
        {
            Normal, EscrowRequest, ChangerPayoutBusy, NoCredit, DefectiveTubeSensor, DoubleArrival,
            AcceptorUnplugged, TubeJam, ROMChecksumError, CoinRoutingError, ChangerBusy, ChangerWasReset,
        }

        public class PSTrade
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

            public int getTransactionFinish()
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
                   1.nSetFunction: int
                   2.nPayoutAmount: int
                   3.nDecPoint: int
                      nSetFunction
                        1->Start Payout
                        2->Check the Inventory amount
                   (b)Return: nSuccess : int
                   (1->Success, other->ErrorCode)
                   The setting function is 0x02, ps3 will calculate the inventory is enough to payout or not. 
                   Controller can order the command (11) to know the result. 
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

        public class PSClassComm
        {
            [DllImport("PS3_DLL.dll", EntryPoint = "Open_ComPort", CallingConvention = CallingConvention.StdCall)]
            static extern bool Open_ComPort(string strComNo, string strBaud);
            [DllImport("PS3_DLL.dll", EntryPoint = "CloseComPort", CallingConvention = CallingConvention.StdCall)]
            static extern bool CloseComPort();
            [DllImport("PS3_DLL.dll", EntryPoint = "Set_ScFa_DePl", CallingConvention = CallingConvention.StdCall)]
            static extern int Set_ScFa_DePl(int ns, int nd);
            [DllImport("PS3_DLL.dll", EntryPoint = "Get_ScFa_DePl", CallingConvention = CallingConvention.StdCall)]
            static extern IntPtr Get_ScFa_DePl();
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_The_Value_of_Bill_Validator", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_The_Value_of_Bill_Validator(int[] nv);
            [DllImport("PS3_DLL.dll", EntryPoint = "Request_The_Value_of_Bill_Validator", CallingConvention = CallingConvention.StdCall)]
            static extern IntPtr Request_The_Value_of_Bill_Validator();
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Each_Type_of_Bill_Enable", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_Each_Type_of_Bill_Enable(int[] nv);
            [DllImport("PS3_DLL.dll", EntryPoint = "Request_Each_Type_of_Bill_Enable", CallingConvention = CallingConvention.StdCall)]
            static extern IntPtr Request_Each_Type_of_Bill_Enable();
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_The_Value_of_Note_Dispenser", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_The_Value_of_Note_Dispenser(int[] nv);
            [DllImport("PS3_DLL.dll", EntryPoint = "Request_The_Value_of_Note_Dispenser", CallingConvention = CallingConvention.StdCall)]
            static extern IntPtr Request_The_Value_of_Note_Dispenser();
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_The_Function_of_Payout_bill", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_The_Function_of_Payout_bill(int[] nv);
            [DllImport("PS3_DLL.dll", EntryPoint = "Request_The_Function_of_Payout_bill", CallingConvention = CallingConvention.StdCall)]
            static extern IntPtr Request_The_Function_of_Payout_bill();
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_the_number_of_bills_in_ND", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_the_number_of_bills_in_ND(int[] nv);
            [DllImport("PS3_DLL.dll", EntryPoint = "Request_the_number_of_bills_in_ND", CallingConvention = CallingConvention.StdCall)]
            static extern IntPtr Request_the_number_of_bills_in_ND();
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Coin_Device_SF_DP", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_Coin_Device_SF_DP(int ns, int nd);
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Coin_Acceptor_parameter", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_Coin_Acceptor_parameter(int[] nv);
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_Hopper_parameter", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_Hopper_parameter(int[] nv);
            [DllImport("PS3_DLL.dll", EntryPoint = "Setting_the_Category_of_Hopper", CallingConvention = CallingConvention.StdCall)]
            static extern int Setting_the_Category_of_Hopper(int nv);

            public enum PSFunc
            {
                Set_ScFa_DePl = 2, Get_ScFa_DePl,
                Setting_The_Value_of_Bill_Validator, Request_The_Value_of_Bill_Validator,
                Setting_Each_Type_of_Bill_Enable, Request_Each_Type_of_Bill_Enable,
                Setting_The_Value_of_Note_Dispenser, Request_The_Value_of_Note_Dispenser,
                Setting_The_Function_of_Payout_bill, Request_The_Function_of_Payout_bill,
                Setting_the_number_of_bills_in_ND, Request_the_number_of_bills_in_ND,
                Setting_Coin_Device_SF_DP, Setting_Coin_Acceptor_parameter,
                Setting_Hopper_parameter, Setting_the_Category_of_Hopper
            }

            public string PSFuncStr(int idx)
            {
                return ((PSFunc)idx).ToString();
            }

            public int[] convertIntPtr(IntPtr ptr)
            {
                int arrayLength = Marshal.ReadInt32(ptr);
                int[] result = new int[arrayLength];
                Marshal.Copy(ptr, result, 0, arrayLength);
                return result;
            }

            public bool PSOpenPort(string ComNo, string bRate = "9600,E,8,1")
            {
                //string ComNo = "COM3";
                return Open_ComPort(ComNo, bRate);
            }

            public bool PSClosePort()
            {
                return CloseComPort();
            }

            public int initPS3()
            {
                /* Set parameters of bill */
                int rtn = setBillFunc();

                if (rtn != PSCons.PS_SUCCESS) return rtn;
                rtn = getBillFunc();
                if (rtn != PSCons.PS_SUCCESS) return rtn;

                return PSCons.PS_SUCCESS;
            }


            private int setBillFunc()
            {
                /* 
                 * Set scaling factor and decimal places of bill
                   (a)Input:
                   1. nScaling : int
                   2. nDecimal : int
                   (b)Return:
                   nSuccess : int
                   (1->Success, other->ErrorCode)
                   (c)Example:
                   int nSuccess = Set_ScFa_DePl(100, 0);    <- TWD, decimal point:0
                */
                if (Set_ScFa_DePl(PSCons.bill_scale_fact, PSCons.decimal_point) != PSCons.PS_SUCCESS) return (int)PSFunc.Set_ScFa_DePl;
                /* 
                 * pBill[0] = 1;鈔票面額(100)除以scaling factor(100)
                   pBill[1] = 2;鈔票面額(200)除以scaling factor(200)
                   pBill[2] = 5;鈔票面額(500)除以scaling factor(500)
                   pBill[3] = 10;鈔票面額(1000)除以scaling factor(1000)
                   …
                   pBill[15] = 0;
                */
                int[] pBill = new int[16] { PSCons.pBAValue_100 / PSCons.bill_scale_fact, PSCons.pBAValue_200 / PSCons.bill_scale_fact, PSCons.pBAValue_500 / PSCons.bill_scale_fact, PSCons.pBAValue_1000 / PSCons.bill_scale_fact, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                if (Setting_The_Value_of_Bill_Validator(pBill) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_The_Value_of_Bill_Validator;
                /* 
                 * pbEnable[0] = 1; <- Enable
                   pbEnable[1] = 1;
                   pbEnable[2] = 1;
                   pbEnable[3] = 1;
                   …
                   pbEnable[15] = 0; <- Disable
                 */
                // Only bill 100 enabled
                int[] pbEnable = new int[16] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                if (Setting_Each_Type_of_Bill_Enable(pbEnable) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_Each_Type_of_Bill_Enable;
                /* Note Dispenser 出鈔機 (ND)
                 * pNDValue [0] = 1;鈔票面額(100)除以scaling factor(100)
                   pNDValue [1] = 0;
                 */
                int[] pNDValue = new int[2] { PSCons.pBAValue_100 / PSCons.bill_scale_fact, 0 };
                if (Setting_The_Value_of_Note_Dispenser(pNDValue) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_The_Value_of_Note_Dispenser;
                /*
                 * pNDEnable[0] = 1; <- Enable
                   pNDEnable[1] = 0; <- Disable
                 */
                int[] pNDEnable = new int[2] { 1, 0 };
                if (Setting_The_Function_of_Payout_bill(pNDEnable) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_The_Function_of_Payout_bill;
                /*
                 * pNDCount[0] = 55; <-- 鈔票面額 100 的有 55 張？
                   pNDCount[1] = 0;
                 */
                int[] pNDCount = new int[2] { 55, 0 };
                if (Setting_the_number_of_bills_in_ND(pNDCount) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_the_number_of_bills_in_ND;
                return PSCons.PS_SUCCESS;
            }

            private int getBillFunc()
            {
                IntPtr ptr = Get_ScFa_DePl();
                int[] pBASfDp = convertIntPtr(ptr);
                if (pBASfDp[0] != PSCons.pBASfDp_100 && pBASfDp[1] != 0) return (int)PSFunc.Get_ScFa_DePl;
                ptr = Request_The_Value_of_Bill_Validator();
                int[] pBAValue = convertIntPtr(ptr);
                return PSCons.PS_SUCCESS;
            }

            private int setCoinFunc()
            {
                /* TWD, decimal point:0*/
                if (Setting_Coin_Device_SF_DP(PSCons.coin_scale_fact, PSCons.decimal_point) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_Coin_Device_SF_DP;
                /*
                 * pCAValue[0] = 1;硬幣面額(1)除以scaling factor(1)
                   pCAValue[1] = 5;硬幣面額(5)除以scaling factor(1)
                   pCAValue[2] = 10;硬幣面額(10)除以scaling factor(1)
                   pCAValue[3] = 50;硬幣面額(50)除以scaling factor(1)
                   …
                   pCAValue[15] = 0;
                 */
                int[] pCAValue = new int[16] { PSCons.pCAValue_1 / PSCons.coin_scale_fact, PSCons.pCAValue_5 / PSCons.coin_scale_fact, PSCons.pCAValue_10 / PSCons.coin_scale_fact, PSCons.pCAValue_50 / PSCons.coin_scale_fact, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                if (Setting_Coin_Acceptor_parameter(pCAValue) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_Coin_Acceptor_parameter;
                /*
                 * pHopper[0] = 1;enable
                   pHopper[1] = 1;
                   pHopper[2] = 0;disable
                   pHopper[3] = 0;
                   pHopper[4] = 5;硬幣面額(5)除以scaling factor(1)
                   pHopper[5] = 10;硬幣面額(10)除以scaling factor(1)
                   pHopper[6] = 0;
                   pHopper[7] = 0;
                  */
                int[] pHopper = new int[8] { 1, 1, 0, 0, PSCons.pCAValue_5 / PSCons.coin_scale_fact, PSCons.pCAValue_10 / PSCons.coin_scale_fact, 0, 0 };
                if (Setting_Hopper_parameter(pHopper) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_Hopper_parameter;
                /*
                 */
                if (Setting_the_Category_of_Hopper(1) != PSCons.PS_SUCCESS) return (int)PSFunc.Setting_the_Category_of_Hopper;
                return PSCons.PS_SUCCESS;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
