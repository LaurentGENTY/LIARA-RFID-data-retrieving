# WebSocketClientCSharp
Data retrieving application of LIARA's sensors and data processing

- Created and developped by Laurent GENTY, trainee IT specialist at the LIARA during the 2018 summer trimester
- ALL RIGHTS RESERVED : Laurent GENTY

  This project was realized as part of my Computing Science University Technology Degree at Bordeaux (DUT Informatique) during my last semester
  at the CRIAAC laboratory at Chicoutimi (Canada).

  The objective of the project is to build tools to retrieve RFID sensors' data located at the LIARA laboratory : they receive force signal
  from RFID Tags located of different objects in the laboratory and to compare them to retrieved data from the simulator Shima : https://github.com/Iannyck/shima

  We wanted the simulator to generate the most faithful data compared to the real life experiments so that's why those tools we will able
  generate sensors' data and generate different types of graph suchs as 2D graphs, Heatmap, Wireframe, ... AND csv data.

  This project contains a C# project which will be able to generate data in csv files if you bind a specific antenna to the Web Socket Client.
  Then, after you retrieved ALL THE NECESSARY DATA (from all antennas, all distance, all angles... just check the Protocol at :
  https://drive.google.com/open?id=1TB08JN2wnAQReUNQhscYbAWte_s2hVMOCmmyfXoR4io) you can process the retrieved data with the Python script.
  It will generate diffents graphs to understand the data.

Installations :
* python latest version at https://www.python.org/downloads/
  - Install with `pip` these different packages with pip install command : `pip install [package]` :
    * pandas
    * pandas_datareader
    * matplotlib
    * numpy
    * mpl_toolkits

- Any IDE (but developper with Visual Studio Community 2017)

* That's all !

- OPTIONNAL
  - Anaconda Navigator -> Jupyter : thanks to this software, most of the previous libraries will be already installed. However, the use of Jupyter (compilation, coding, etc...) has few defaults such as you won't be able to manipulate interactively the generated graphs.

Use :
  The project is divided in 2 parts : the data's retrieve and the data's process. To organize your experiment protocol you can read (in French though)
  the protocol is followed to retrieve my data with different distances, different angles, etc... at   https://drive.google.com/open?id=1TB08JN2wnAQReUNQhscYbAWte_s2hVMOCmmyfXoR4io

  - First step : retrieve the data
    - First of all, you will need to place an object on a specific position in the laboratory : for instance at 10 cm from the first antenna. When you made a specific configuration in the laboratory you we be able to start the C# project.

    - You can launch the C# project in the folder : `WebSocketClientCSharp\WebSocketClient\WebSocketClient\bin\Debug\WebSocketClient.exe` (or start the project with Visual Studio then Run the application). Then, if you want the WebSocketClient to work perfectly, you must connect your computer to the Wi-Fi : LIARA (login : Liara, Password : liara_lab). Now you're ready to retrieve your data !

    - In the interface you have to enter the IP Adress of the LIARA network (normally, it's already written) and you have to choose :
      - current Antenna (the one you want to retrieve its data)
      - current Object (the one you're doing your experiment with)
      - current Distance
      - current Angle

    - Then you can start the project by clicking on the OPEN Button : the program will retrieve 100 sample of the antenna's data. The program will generate all the data in the following folder : WebSocketClientCSharp\WebSocketClient\WebSocketClient\bin\Debug\xp

    - Do this manipulation for every combination of Distance/Angle (like that, you will have EVERY NECESSARY data to generate the following graphs)

  - Second step : data's process
    - Now you can generate every graphs ! I mean, the Python script will do it for you ! To generate those graphs you have 2 choices depending on how you did with your installations :
      - if you use Python, just run the Python script at `WebSocketClientCSharp\WebSocketClient\WebSocketClient\bin\Debug\generate\generateScript.py`(make sure you got all libraries installed on your computer) : it will generate every graphs AND you will be able to interact with them :
        - rotate
        - zoom
        - change scale
        - ...

      WARNING : If you have an error when you're trying to import pandas_datareader in `fred.py`, follow these instructions found on this StackOverflow post : https://stackoverflow.com/questions/50394873/import-pandas-datareader-gives-importerror-cannot-import-name-is-list-like

      - if you want to use Jupyter Notebook, launch Anaconda Navigator and click on Jupyter Notebook. It will open a website page where you we need this folder : `WebSocketClientCSharp\WebSocketClient\WebSocketClient\bin\Debug\generate` IPython files will be in this folder.

      Then open `generateScript.ipynb` and Run the program.
      WARNING : if you use this method, you won't be able to interact with previous graphs

      This step will generate all the resulted graphs we talked before in the folder : `WebSocketClientCSharp\WebSocketClient\WebSocketClient\bin\Debug\xp\data`

      - Now you can find different resulted files, such as :
        - `WebSocketClientCSharp\WebSocketClient\WebSocketClient\bin\Debug\xp\data` :
          - `data.csv` : a csv file which contains all information relative to the data such as :
            - average of RSSI values for every couple distance/angle
            - number of samples for every experiments
            - standard deviation
            - average of time for a sample

            * format of `data.csv` : `[Name of the experiment[Object]_[distance]_[angle]];[Total of Samples];[Average];[Standard Deviation]`

          - `finalData.csv` : the final csv file which contains every average of RSSI values for every couple distance/angle. With this file the python script will generate the heatmap and the wireframe

          - format of finalData.csv : double entry table : `Distance;0[°];23[°];45[°];90[°];120[°];180[°]`

        - 2d/ :
          - every 2D graphs (as PNG) which shows us the divition between every samples during the experiment AND the average and the experiment for every couple distance/angle

            - format of PNG files : `[Object]_[distance]_[angle].png`

        - 2ddistanceangle/ :
          - the recap between every curves for each couple distance/angle

            - format of `output.png` : 6 curves (one for each angle previously quoted) ; `XAxis : distance / YAxis : RSSI Value`

        - 3d/ :
          - the recap in 3D (as a WireFrame graph) of all average of the couple distance/angle

            - format of `3d.png` : 78 dots in 3d for every couple distance/angle ; `XAxis : Angle / YAxis : Distance / ZAxis : RSSI Value`

            WARNING : remember, if you use Python script to execute the script, it will be easier to read the data on this WireFrame (because you will be able to rotate, zoom, change the scale...)

        - heatmap/ :
          - the resulted heatmap generated with the `finalData.csv` file : average of every RSSI value for every couple distance/angle

            - format of `heatmap.png` : a scale of colours for every RSSI value on the right of the heatmap ; `XAxis : Distance / YAxis : Angle`


TO REMEMBER :
  - if you want to generate graphs and stuff with the python script REMEMBER to retrieve data for every couple distance/angle otherwise
  you won't be able to generate the graphs
  - the pandas_datareader error is very important because you won't be able to to execute the python script
