namespace NanoTwitchBot.Core.Nano
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NanoTwitchBot.Core.Nano.Model;

    public class NanoListener
    {
        #region Constructors

        public NanoListener(NanoClient nanoClient, string account, TimeSpan interval, int count)
        {
            this.Nano = nanoClient;
            this.Account = account;
            this.Interval = interval;
            this.Count = count;

            // Interval
            if (this.Interval < TimeSpan.FromSeconds(1))
            {
                this.Interval = TimeSpan.FromSeconds(1);
            }

            // Known Hashes
            this.knownHashes = new List<string>();
        }

        #endregion

        #region Fields

        private List<string> knownHashes;

        #endregion

        #region Properties

        public string Account { get; private set; }

        public int Count { get; private set; }

        public TimeSpan Interval { get; private set; }

        public NanoClient Nano { get; private set; }

        #endregion

        #region Methods

        public Task Start(Action<NanoListenerTransaction> transactionCallback, Action<NanoResult> errorCallback)
        {
            return Task.Run(
                async () =>
                {
                    while (true)
                    {
                        try
                        {
                            await Task.Delay(this.Interval);
                            PendingRequest pendingRequest = new PendingRequest();
                            pendingRequest.Account = this.Account;
                            pendingRequest.Count = this.Count.ToString();
                            NanoResult<PendingResponse> pendingResult = await this.Nano.SendAsync<PendingResponse>(pendingRequest);
                            if (pendingResult.IsError())
                            {
                                errorCallback?.Invoke(pendingResult);
                            }

                            BlocksInfoRequest blocksInfoRequest = new BlocksInfoRequest();
                            blocksInfoRequest.Hashes = pendingResult.Response.Blocks;
                            NanoResult<BlocksInfoResponse> blocksInfoResult = await this.Nano.SendAsync<BlocksInfoResponse>(blocksInfoRequest);
                            if (blocksInfoResult.IsError())
                            {
                                errorCallback?.Invoke(blocksInfoResult);
                            }

                            if (blocksInfoResult.Response.Blocks != null)
                            {
                                foreach ((string key, BlockInfo block) in blocksInfoResult.Response.Blocks)
                                {
                                    if (this.knownHashes.Contains(key))
                                    {
                                        continue;
                                    }

                                    if (block.Confirmed == "true")
                                    {
                                        this.knownHashes.Add(key);
                                        transactionCallback?.Invoke(new NanoListenerTransaction(block));
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // Do Nothing (For Now)
                        }
                    }
                });
        }

        #endregion
    }
}