/*
* Copyright (c) 2025 Original Author(s), PhonePe India Pvt. Ltd.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

namespace pg_sdk_dotnet.Common.Models.Request;

/**
 * Used to create an Account Constraint which can be send with the pay request
 */

 public class AccountConstraint(string accountNumber, string ifsc) : InstrumentConstraint(PaymentInstrumentType.ACCOUNT)
{
  public string AccountNumber { get; } = accountNumber;
  public string Ifsc { get; } = ifsc;

  public static AccountConstraintBuilder Builder()
  {
      return new AccountConstraintBuilder();
  }
}

public class AccountConstraintBuilder
{
    private string? _accountNumber;
    private string? _ifsc;

    public AccountConstraintBuilder AccountNumber(string accountNumber)
    {
        this._accountNumber = accountNumber;
        return this;
    }

    public AccountConstraintBuilder Ifsc(string ifsc)
    {
        this._ifsc = ifsc;
        return this;
    }

    public AccountConstraint Build()
    {
        return new AccountConstraint(this._accountNumber!, this._ifsc!);
    }
}