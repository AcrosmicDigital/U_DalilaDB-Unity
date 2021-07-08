using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using U.DalilaDB;
using UnityEngine;
using UnityEngine.TestTools;

public class Dalila_DataOperation
{

    [Test]
    public void A_Successful_CanBeSetToSuccessOnlyOnce()
    {

        // When an altern operation is created
        var operation = new DataOperation();
        var operationWithMessage = new DataOperation();

        // You can set to Success
        operation.Successful();
        operationWithMessage.Successful("A little error found");

        // And will be true and exception null and be set
        Assert.IsTrue(operation.IsDone);
        Assert.IsTrue(operation);
        Assert.Null(operation.Error);
        Assert.IsTrue(operation.Message == "");

        Assert.IsTrue(operationWithMessage.IsDone);
        Assert.IsTrue(operationWithMessage);
        Assert.Null(operationWithMessage.Error);
        Assert.IsTrue(operationWithMessage.Message == "A little error found");


        // You cant set again the operation
        Assert.Throws<DataOparationAlreadySetException>(() => operation.Successful());
        Assert.Throws<DataOparationAlreadySetException>(() => operation.Fails(new System.Exception()));

        Assert.Throws<DataOparationAlreadySetException>(() => operationWithMessage.Successful());
        Assert.Throws<DataOparationAlreadySetException>(() => operationWithMessage.Fails(new System.Exception()));

    }


    [Test]
    public void B_Fails_CanBeSetToFailedOnlyOnce()
    {

        // When an altern operation is created
        var operation = new DataOperation();

        // You can set to failed
        operation.Fails(new System.Exception());

        // And will be true and exception null and be set
        Assert.IsTrue(operation.IsDone);
        Assert.IsFalse(operation);
        Assert.NotNull(operation.Error);
        Assert.IsTrue(operation.Message == "");


        // You cant set again the operation
        Assert.Throws<DataOparationAlreadySetException>(() => operation.Successful());
        Assert.Throws<DataOparationAlreadySetException>(() => operation.Fails(new System.Exception()));

    }


    [Test]
    public void C_ImplicitOperatorBool()
    {

        // When an altern operation is created
        var successDataOperation = new DataOperation();
        var failedDataOperation = new DataOperation();
        var uncompletedDataOperation = new DataOperation();

        // You can set to failed or success
        successDataOperation.Successful();
        failedDataOperation.Fails(new System.Exception("Expected exception"));

        // And will be true and exception null and be set
        Assert.IsTrue(successDataOperation);
        Assert.IsFalse(failedDataOperation);
        Assert.IsFalse(uncompletedDataOperation);

    }


    [Test]
    public void D_ImplicitOperatorString()
    {
        // When an altern operation is created
        var successDataOperation = new DataOperation();
        var successDataOperationWithMessage = new DataOperation();
        var failedDataOperation = new DataOperation();
        var uncompletedDataOperation = new DataOperation();

        // You can set to failed
        successDataOperation.Successful();
        successDataOperationWithMessage.Successful("A little error found");
        failedDataOperation.Fails(new System.Exception("Expected exception"));

        LogAssert.Expect(LogType.Assert, "The operation is: Completed successfully: ");
        LogAssert.Expect(LogType.Assert, "The operation is: Completed successfully: A little error found");
        LogAssert.Expect(LogType.Assert, "The operation is: Failed: System.Exception: Expected exception");
        LogAssert.Expect(LogType.Assert, "The operation is: In progress");

        Debug.LogAssertion("The operation is: " + successDataOperation);
        Debug.LogAssertion("The operation is: " + successDataOperationWithMessage);
        Debug.LogAssertion("The operation is: " + failedDataOperation);
        Debug.LogAssertion("The operation is: " + uncompletedDataOperation);

    }


    [Test]
    public void E_ImplicitOperatorInt()
    {
        // When an altern operation is created
        var successDataOperation = new DataOperation();
        var successDataOperationTwo = new DataOperation();
        var successDataOperationThree = new DataOperation();
        var successDataOperationFour = new DataOperation();
        var successDataOperationFive = new DataOperation();
        var successDataOperationSix = new DataOperation();
        var failedDataOperation = new DataOperation();
        var uncompletedDataOperation = new DataOperation();

        // You can set to failed
        successDataOperation.Successful("0");
        successDataOperationTwo.Successful("12");
        successDataOperationThree.Successful("-22");
        successDataOperationFour.Successful();
        successDataOperationFive.Successful("--12");
        successDataOperationSix.Successful("Other text");
        failedDataOperation.Fails(new System.Exception("Expected exception"));


        Assert.AreEqual(0, (int)successDataOperation);
        Assert.AreEqual(12, (int)successDataOperationTwo);
        Assert.AreEqual(-22, (int)successDataOperationThree);
        Assert.AreEqual(0, (int)successDataOperationFour);
        Assert.AreEqual(0, (int)successDataOperationFive);
        Assert.AreEqual(0, (int)successDataOperationSix);
        Assert.AreEqual(0, (int)failedDataOperation);
        Assert.AreEqual(0, (int)uncompletedDataOperation);

        Assert.IsTrue(successDataOperation == 0);
        Assert.IsTrue(successDataOperationTwo == 12);
        Assert.IsTrue(successDataOperationThree == -22);

    }


    [Test]
    public void F_ImplicitOperatorFloat()
    {
        // When an altern operation is created
        var successDataOperation = new DataOperation();
        var successDataOperationTwo = new DataOperation();
        var successDataOperationThree = new DataOperation();
        var successDataOperationFour = new DataOperation();
        var successDataOperationFive = new DataOperation();
        var successDataOperationSix = new DataOperation();
        var failedDataOperation = new DataOperation();
        var uncompletedDataOperation = new DataOperation();

        // You can set to failed
        successDataOperation.Successful("0");
        successDataOperationTwo.Successful("12.22");
        successDataOperationThree.Successful("-22.11");
        successDataOperationFour.Successful();
        successDataOperationFive.Successful("--12.22");
        successDataOperationSix.Successful("Other text");
        failedDataOperation.Fails(new System.Exception("Expected exception"));

        Assert.AreEqual(0f, (float)successDataOperation);
        Assert.AreEqual(12.22f, (float)successDataOperationTwo);
        Assert.AreEqual(-22.11f, (float)successDataOperationThree);
        Assert.AreEqual(0f, (float)successDataOperationFour);
        Assert.AreEqual(0f, (float)successDataOperationFive);
        Assert.AreEqual(0f, (float)successDataOperationSix);
        Assert.AreEqual(0f, (float)failedDataOperation);
        Assert.AreEqual(0f, (float)uncompletedDataOperation);

        Assert.IsTrue(successDataOperation == 0f);
        Assert.IsTrue(successDataOperationTwo == 12.22f);
        Assert.IsTrue(successDataOperationThree == -22.11f);

    }

}
