using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketClient
{
    class TW
    {
        public Task t { get; set; }
        public bool stop { get; set; }
        public WebSocketClient form;
        public int sec { get; set; }
        public int min { get; set; }
        public int totalSec { get; set; }

        public TW(WebSocketClient form)
        {
            this.form = form;
            t = new Task(Todo);
        }

        public void Todo()
        {
            sec = 0;
            min = 0;
            stop = false;
            totalSec = sec;
            string final;

            do
            {
                final = min + " min " + sec + " sec";

                form.updateTime(final);
                Thread.Sleep(1000);
                sec++;
                totalSec++;
                if (sec == 60)
                {
                    sec = 0;
                    min++;
                }
            } while (totalSec < form.getTimeLimit()+1 && !stop);

            //Si le thread s'est terminé car on a atteint la limite de temps total à savoir 5 min alors envoie la dialogue box sinon non
            if(!stop)
            {
                form.end();
            }
        }

        public void Run()
        {
            t.Start();
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
