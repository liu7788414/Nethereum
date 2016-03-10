using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Xunit;

namespace Nethereum.Web3.Sample
{

    public class IntTypeIntegrationTests
    {
        public async Task<string> Test()
        {
            //The compiled solidity contract to be deployed
            /*
               contract test { 

               uint _multiplier;

               function test(uint multiplier){
                   _multiplier = multiplier;
               }

               function getMultiplier() constant returns(uint d){
                    return _multiplier;
               }

               function multiply(uint a) returns(uint d) { return a * _multiplier; }

               string public contractName = "Multiplier";
           }
           */

            var contractByteCode =
                "60606040526102b7806100126000396000f36060604052361561008a576000357c01000000000000000000000000000000000000000000000000000000009004806311da9d8c1461008c5780631c2a1101146100b857806363798981146100e45780636b59084d146101105780639e71212514610133578063a605861c1461015f578063e42d455b1461018b578063e92b09da146101b75761008a565b005b6100a26004808035906020019091905050610243565b6040518082815260200191505060405180910390f35b6100ce600480803590602001909190505061020e565b6040518082815260200191505060405180910390f35b6100fa60048080359060200190919050506101ff565b6040518082815260200191505060405180910390f35b61011d60048050506101e3565b6040518082815260200191505060405180910390f35b6101496004808035906020019091905050610229565b6040518082815260200191505060405180910390f35b6101756004808035906020019091905050610274565b6040518082815260200191505060405180910390f35b6101a1600480803590602001909190505061029e565b6040518082815260200191505060405180910390f35b6101cd6004808035906020019091905050610287565b6040518082815260200191505060405180910390f35b6000600068bb75377716692498d690508091506101fb565b5090565b6000819050610209565b919050565b60006000600160018401039050809150610223565b50919050565b600068bb75377716692498d68214905061023e565b919050565b60007fffffffffffffffffffffffffffffffffffffffffffffff448ac888e996db672a8214905061026f565b919050565b60006101f482149050610282565b919050565b60006544247b660f3d82149050610299565b919050565b6000678000000000000000821490506102b2565b91905056";

            var abi =
                @"[{""constant"":false,""inputs"":[{""name"":""d"",""type"":""int256""}],""name"":""test5"",""outputs"":[{""name"":"""",""type"":""bool""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""d"",""type"":""int256""}],""name"":""test3"",""outputs"":[{""name"":"""",""type"":""int256""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""d"",""type"":""int256""}],""name"":""test2"",""outputs"":[{""name"":"""",""type"":""int256""}],""type"":""function""},{""constant"":false,""inputs"":[],""name"":""test1"",""outputs"":[{""name"":"""",""type"":""int256""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""d"",""type"":""int256""}],""name"":""test4"",""outputs"":[{""name"":"""",""type"":""bool""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""d"",""type"":""int256""}],""name"":""test6"",""outputs"":[{""name"":"""",""type"":""bool""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""d"",""type"":""int256""}],""name"":""test8"",""outputs"":[{""name"":"""",""type"":""bool""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""d"",""type"":""int256""}],""name"":""test7"",""outputs"":[{""name"":"""",""type"":""bool""}],""type"":""function""}]";

            var addressFrom = "0x12890d2cce102216644c59dae5baed380d84830c";

            var web3 = new Web3();

          
            var pass = "password";
            var result = await web3.Personal.UnlockAccount.SendRequestAsync(addressFrom, pass, new HexBigInteger(600));

            //deploy the contract, including abi and a paramter of 7. 
            var transactionHash =
                await
                    web3.Eth.DeployContract.SendRequestAsync(contractByteCode, addressFrom,
                        new HexBigInteger(900000));

            //get the contract address 
            TransactionReceipt receipt = null;
            //wait for the contract to be mined to the address
            while (receipt == null)
            {
                await Task.Delay(500);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }

            var contract = web3.Eth.GetContract(abi, receipt.ContractAddress);
            var test1 = contract.GetFunction("test1");
            Assert.Equal("3457987492347979798742", (await test1.CallAsync<BigInteger>()).ToString());
            var test2 = contract.GetFunction("test2");
            Assert.Equal("3457987492347979798742", (await test2.CallAsync<BigInteger>(BigInteger.Parse("3457987492347979798742"))).ToString());

            var test3 = contract.GetFunction("test3");
            Assert.Equal("3457987492347979798742", (await test3.CallAsync<BigInteger>(BigInteger.Parse("3457987492347979798742"))).ToString());

            var test4 = contract.GetFunction("test4");
            Assert.True(await test4.CallAsync<bool>(BigInteger.Parse("3457987492347979798742")));

            var test5 = contract.GetFunction("test5");
            Assert.True(await test5.CallAsync<bool>(BigInteger.Parse("-3457987492347979798742")));

            var test6 = contract.GetFunction("test6");
            Assert.True(await test6.CallAsync<bool>(BigInteger.Parse("500")));

            var test8 = contract.GetFunction("test8");
            Assert.True(await test8.CallAsync<bool>(BigInteger.Parse("9223372036854775808")));




            return "OK";
        }
    }

        public class ContractConstructorDeploymentAndCall
    {
        public async Task<string> Test()
        {
            //The compiled solidity contract to be deployed
            /*
               contract test { 

               uint _multiplier;

               function test(uint multiplier){
                   _multiplier = multiplier;
               }

               function getMultiplier() constant returns(uint d){
                    return _multiplier;
               }

               function multiply(uint a) returns(uint d) { return a * _multiplier; }

               string public contractName = "Multiplier";
           }
           */

            var contractByteCode =
                "0x6060604052604060405190810160405280600a81526020017f4d756c7469706c6965720000000000000000000000000000000000000000000081526020015060016000509080519060200190828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061008c57805160ff19168380011785556100bd565b828001600101855582156100bd579182015b828111156100bc57825182600050559160200191906001019061009e565b5b5090506100e891906100ca565b808211156100e457600081815060009055506001016100ca565b5090565b5050604051602080610303833981016040528080519060200190919050505b806000600050819055505b506101e2806101216000396000f360606040526000357c01000000000000000000000000000000000000000000000000000000009004806340490a901461004f57806375d0c0dc14610072578063c6888fa1146100ed5761004d565b005b61005c6004805050610119565b6040518082815260200191505060405180910390f35b61007f6004805050610141565b60405180806020018281038252838181518152602001915080519060200190808383829060006004602084601f0104600f02600301f150905090810190601f1680156100df5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b610103600480803590602001909190505061012b565b6040518082815260200191505060405180910390f35b60006000600050549050610128565b90565b60006000600050548202905061013c565b919050565b60016000508054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156101da5780601f106101af576101008083540402835291602001916101da565b820191906000526020600020905b8154815290600101906020018083116101bd57829003601f168201915b50505050508156";

            var abi =
                @"[{""constant"":true,""inputs"":[],""name"":""getMultiplier"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""contractName"",""outputs"":[{""name"":"""",""type"":""string""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""uint256""}],""type"":""constructor""}]";

            var addressFrom = "0x12890d2cce102216644c59dae5baed380d84830c";

            var web3 = new Web3();

            //deploy the contract, including abi and a paramter of 7. 
            var transactionHash = await web3.Eth.DeployContract.SendRequestAsync(abi, contractByteCode, addressFrom, new HexBigInteger(900000), 7);

            //the contract should be mining now

            //get the contract address 
            TransactionReceipt receipt = null;
            //wait for the contract to be mined to the address
            while (receipt == null)
            {
                await Task.Delay(500);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }

            var contract = web3.Eth.GetContract(abi, receipt.ContractAddress);

            //get the function by name
            var multiplyFunction = contract.GetFunction("multiply");

            //do a function call (not transaction) and get the result
            var result = await multiplyFunction.CallAsync<int>(69);

            var multiplierFunction = contract.GetFunction("getMultiplier");

            var multiplier = await multiplierFunction.CallAsync<int>();

            var contractNameFunction = contract.GetFunction("contractName");
            var name = await contractNameFunction.CallAsync<string>();

            //visual test 
            return "The result of deploying a contract and calling a function to multiply " + multiplier + " by 69 is: " + result +
                   " and should be 483";
        }
    }
}