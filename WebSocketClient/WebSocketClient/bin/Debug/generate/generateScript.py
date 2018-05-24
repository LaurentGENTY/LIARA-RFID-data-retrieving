
# coding: utf-8

# In[20]:


import pandas as pd
from pandas import DataFrame
import numpy as np

import datetime
import pandas_datareader as pdr
from mpl_toolkits.mplot3d import axes3d
import matplotlib.pyplot as plt

import glob, os

#fonction qui renvoie le bon path
def getPath() :
    
    #Path courant
    cwd = os.getcwd()
    #C:\Users\laure\WebSocketClientCSharp\WebSocketClient\generate
    print(cwd)

    #On va changer de path (cwd, '../WebSocketClient/bin/Debug/xp/'))
    os.chdir('../xp/')
    print(os.getcwd())

    return;

def finalDataExist() :
    #On va remplir le fichier final : tester s'il existe
    #Si le fichier n'existe pas on le crée
    
    if os.path.isfile('data/finalData.csv') is True:
        os.remove('data/finalData.csv')
        
    open('data/finalData.csv', 'a').close()        
        
    #On écrit le nom des colonnes
    with open('data/finalData.csv', 'a') as the_file:
        the_file.write('Distance;0;23;45;90;120;180')
        the_file.write('\n')
    #else:
        
    return;

#----------- TO DO -------------
#Fonction permettant de delete tous les précédentes data
def clearAll() :
    return;
    
def generationHeatMap() :
    
    #Separé par des ; donc l'index est la première colonne
    df = pd.read_csv('data/finalData.csv',sep=';',index_col=0)

    #notre X sera l'index : la distance
    x = df.index

    #notre Y sera l'index : la distance
    y = df.columns.astype(int)

    df.values.astype(float)
    z = df.values

    X,Y = np.meshgrid(x,y)
    Z = z.T
    plt.contourf(X,Y,Z,cmap='jet')

    plt.colorbar()
    plt.savefig('data/heatmap/{}.png'.format('heatmap'))

    plt.show()
    plt.close()

    return;

def generation2DDistanceAngle() :
    
    #Separé par des ; donc l'index est la première colonne
    df = pd.read_csv('data/finalData.csv',sep=';',index_col=0)

    #print(df)

    df=df.astype(float)
    
    fig = df.plot().get_figure()
    fig.savefig("data/2ddistanceangle/output.png")
    plt.show(block=False)
    plt.close(fig)
    
    return;

def generationWireFrame() :
    
    raw_data = np.loadtxt('data/finalData.csv', delimiter=';', dtype=np.string_)

    angle    = raw_data[0 , 1:].astype(float)
    distance = raw_data[1:, 0 ].astype(float)
    data     = raw_data[1:, 1:].astype(float)


    #Données à l'intérieur de la matrice
    data = angle[np.newaxis,:] / distance[:,np.newaxis] 

    #Setup du plot
    fig = plt.figure()
    ax = fig.add_subplot(111, projection='3d')

    #Créer les données que le wireframe souhaite
    Z = data
    X, Y = np.meshgrid(angle, distance)

    ax.plot_wireframe(X, Y, Z)

    #Legendes
    ax.set_xticks(angle)
    ax.set_yticks(distance[::2])
    ax.set_xlabel('angle')
    ax.set_ylabel('distance')

    #On la'ffiche et on l'enregistre
    plt.savefig('data/3d/3d.png')
    plt.show()


    return;

#------------------------------------------------------------------
#on va générer les csv des moyennes pour chaque Angle
#On va parcourir tous les CSV ayant la même distance pour un angle
#Faire la moyenne des données dans le fichier CSV
#...
#------------------------------------------------------------------

#On change le directory
getPath()

#Liste des angles permettant d'effectuer les lectures pour les algo
distances = ['0','5','10','15','25','40','60','80','100','120','140','160','180','200']

finalDataExist()

for (i, distance) in enumerate(distances):
    #i = index
    #distance = distance courante
    
    #Nous allons stocker les moyennes des RSSI pour chaque couple distance/angle
    averagesRSSI = {}
    
    for file in glob.glob("*.csv"):
        #nom du fichier
        #print(file)
        
        #On sépare le nom du fichier pour savoir si l'angle est le bon
        parts = file.split('_')
        
        #On récupère la distance courante
        #Avec cette distance on va lire tous les fichiers qui concernent cette distance
        #créer une ligne pour append dans le csv final
        
        #Alors on va lire le fichier
        
        if parts[1] == distance:  
            print(file)
            filePath = os.getcwd().replace("\\","/")+"/"+file

            # --------------- CONSTRUCTION GRAPHE 2D RSSI / TIME -------------------------
            
            df = pd.read_csv(filePath,sep=';',index_col=0,parse_dates=True)

            #Moyenne -------------
            #On dit que c'est un float
            df['rssi1'] =df.astype(float)

            #On fait la moyenne de la première colonne
            average = df['rssi1'].mean()
            nb = df.shape[0]
            
            #On enregistre l'average RSSI dans le tableau pour l'append dans le finalData.csv
            #EXEMPLE : Sel_0_0.csv alors averageRSSI[0'°'] = moyenne courante
            #Sel_0_23.csv : averageRSSI[23] = moyenne courante
            #...
            averagesRSSI[file.split('_')[2].split('.')[0]] = average
            
            #On va plot (dessiner) le graph de l'évolution du RSSI selon le temps
            plt.ylabel('RSSI')
            plt.grid(True)
            df['rssi1'].plot()
            
            #On affiche la moyenne dans le plot
            plt.axhline(y=average, color='r', linestyle='-')
            
            temp = file.split('.')[0]
            plt.savefig('data/2d/'+temp+'.png')
            plt.close()
            #Si le fichier n'existe pas on le crée
            if os.path.isfile('data/data.csv') is False:
                open('data/data.csv', 'a').close()
                
                #On écrit le nom des colonnes
                with open('data/data.csv', 'a') as the_file:
                    the_file.write('Nom;Echantillons;Moyenne;EcartType;MoyenneTemps')
                    the_file.write('\n')        
                
            #On écrit les infos
            #Le nom du fichier + nombre d'échantillons + la moyenne + écart type + [autres infos]
            with open('data/data.csv', 'a') as the_file:
                the_file.write(temp+';'+str(nb)+';'+str(average)+';'+'0'+';'+'0')
                the_file.write('\n')
                
            #----------------------------------------------------------------------
            
                
        del parts
    
    print(averagesRSSI)
    #Maintenant que notre dictionnary des moyennes pour chaque couple Distance / Angle
    
    if bool(averagesRSSI) is True:
        final = distance + ';' + str(averagesRSSI["0"]) + ';'+ str(averagesRSSI["23"]) + ';' + str(averagesRSSI["45"]) + ';' + str(averagesRSSI["90"]) + ';' + str(averagesRSSI["120"]) + ';' + str(averagesRSSI["180"])
        print(final)
        
        with open('data/finalData.csv', 'a') as the_file:
                the_file.write(final)
                the_file.write('\n')
    
    
    
    del averagesRSSI


generation2DDistanceAngle()
    
generationHeatMap()

generationWireFrame()

