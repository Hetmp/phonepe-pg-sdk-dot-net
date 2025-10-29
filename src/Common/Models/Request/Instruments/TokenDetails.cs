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

namespace pg_sdk_dotnet.Common.Models.Request.Instruments;

public class TokenDetails
{
    public string EncryptedToken { get; }
    public string EncryptedCvv { get; }
    public long EncryptionKeyId { get; }
    public Expiry Expiry { get; }
    public string Cryptogram { get; }
    public string PanSuffix { get; }
    public string? CardHolderName { get; }
    public string? Eci { get; }
    public string? Atc { get; }

    private TokenDetails(
        string encryptedToken,
        string encryptedCvv,
        long encryptionKeyId,
        Expiry expiry,
        string cryptogram,
        string panSuffix,
        string? cardHolderName,
        string? eci,
        string? atc)
    {
        EncryptedToken = encryptedToken;
        EncryptedCvv = encryptedCvv;
        EncryptionKeyId = encryptionKeyId;
        Expiry = expiry;
        Cryptogram = cryptogram;
        PanSuffix = panSuffix;
        CardHolderName = cardHolderName;
        Eci = eci;
        Atc = atc;
    }

    public static TokenDetailsBuilder Builder()
    {
        return new TokenDetailsBuilder();
    }

    public class TokenDetailsBuilder
    {
        private string _encryptedToken = string.Empty;
        private string _encryptedCvv = string.Empty;
        private long _encryptionKeyId;
        private Expiry? _expiry;
        private string _cryptogram = string.Empty;
        private string _panSuffix = string.Empty;
        private string? _cardHolderName;
        private string? _eci;
        private string? _atc;

        /**
          * SETTERS
          */
        public TokenDetailsBuilder SetEncryptedToken(string encryptedToken)
        {
            this._encryptedToken = encryptedToken;
            return this;
        }

        public TokenDetailsBuilder SetEncryptedCvv(string encryptedCvv)
        {
            this._encryptedCvv = encryptedCvv;
            return this;
        }

        public TokenDetailsBuilder SetEncryptionKeyId(long encryptionKeyId)
        {
            this._encryptionKeyId = encryptionKeyId;
            return this;
        }

        public TokenDetailsBuilder SetExpiry(Expiry expiry)
        {
            this._expiry = expiry;
            return this;
        }

        public TokenDetailsBuilder SetCryptogram(string cryptogram)
        {
            this._cryptogram = cryptogram;
            return this;
        }

        public TokenDetailsBuilder SetPanSuffix(string panSuffix)
        {
            this._panSuffix = panSuffix;
            return this;
        }

        public TokenDetailsBuilder SetCardHolderName(string? cardHolderName)
        {
            this._cardHolderName = cardHolderName;
            return this;
        }

        public TokenDetailsBuilder SetEci(string? eci)
        {
            this._eci = eci;
            return this;
        }

        public TokenDetailsBuilder SetAtc(string? atc)
        {
            this._atc = atc;
            return this;
        }

        public TokenDetails Build()
        {          
            return new TokenDetails(
                this._encryptedToken,
                this._encryptedCvv,
                this._encryptionKeyId,
                this._expiry!,
                this._cryptogram,
                this._panSuffix,
                this._cardHolderName,
                this._eci,
                this._atc
            );
        }
    }
}