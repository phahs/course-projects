# Project #2 CPSC 335
# Group Members: Adam Rounds, Philip Hahs, Karine Kuilanoff
# MW 4PM
# due: 10/9/14


print('CS 335 Project 2 - Exhaustive Search/Knapsack')
import sys
import time


########################Pseudocode#####################################

#def Exhaustive_Knapsack(package_list, W):
    #Best = []
    #For candidate in subsets(package_list):
        #if verify_knapsack(candidate, W):
            #if compare_knapsack(candidate, best):
                #Best.append(candidate)
                #W-= b
    #return Best

    #function to load packages into a list, read in from text file
def Subset(list):
   ##Make the class list and store the info into corresponding fields
    power_list = [[]]   #empty list to be filled with packages
    for i in list: #this will probably be total_packages
        with_i =[]
        for s in power_list:
            t = s +[i]
            with_i.append(t)
        power_list = power_list + with_i

    return power_list #return the populated list

def Verify_Knapsack(candidate, W):
    total_weight = 0
    for item in candidate:
       total_weight += item.size
    return (total_weight <= W)

def total_value(list):
   value = 0
   for item in list:
       value += item.votes
   return value

def knapsack(package_list, n, W):
    best = None
    for candidate in package_list:
        if Verify_Knapsack(candidate, W):
            if best is None or total_value(candidate) > total_value(best):
                best = candidate
    return best
########################################################################################
#TIME INFO:
#useful time function stuff-
#    start = time.perf_counter()  
#    end = time.perf_counter()
####################################################################################

#class to define the package, recommended by professor in pdf

class DebianPackage():
    #fields for the class, you use statements not like C++ for fields 
    name = ""
    votes = 0 
    size = 0

#####################################################################################
#sys.argv should = 4 for this project, so updated the checker statement
#sys.argv[0] = CS_335_Project_2.py
#sys.argv[1] = packages.txt
#sys.argv[2] = n
#sys.argv[3] = W
# n = this IS the number of best fitting package
# W = maximum capacity
#######################################################################################


def main():
##########Visual Studio already has sys.argv[0] (this source code file name) in its Debug properties so don't include it###################
    #this is a checker to make sure the number of arguments being sent in from the command line is good
    if len(sys.argv) != 4:
        print('error: you must supply exactly three arguments\n\n' +
              'usage: python3  <text file> <n> <W> ')     # usage: python3 <Python source file> <text file> <n> <W>
                                                       
                                                          #<python source file is already included when you run it (due to Debug properties setup)
                                                          #so you don't need to include it in the command line, 
                                                          #just put the textfile and n and W's value in the Debug Properties->Script Arguments
#        sys.exit(1)

    n = int(sys.argv[2])        #size of the knapsack
    w = int(sys.argv[3])        #capacity, max storage
    
    filename = sys.argv[1]      #this is the text file we are reading from! should be packages.txt 
                                #have it set up in the Debug properties-> Script arguments
    file = open(filename) #this opens the input file for reading
   
    total_packages = int(file.readline()) #reads the first line in the file, 36149, stores into total_packages converted to an int
    
    print('Loaded ' + filename)   # says which file we have opened, then displays the length of the file
    print ( "First line of the file is: " + str(total_packages) + "\n\n") # this prints the first line
    num_packages = total_packages
    list = []
    for i in range(n): #this will probably be total_packages
        list.append(DebianPackage())
               ################################READY TO READ IN DATA###########################################################################
        info = file.readline().split(" ")   ##this makes a list for each line being read in! When read in white space it splits the line up into tokens
                                            ##So in this project, the lines will be turned into a list of 3 components:
                                            ##list[0] = name of package
                                            ##list[1] = size of package
                                            ##list[2] = votes for package
    ####Getting the information sotred into the class fields at this point#################
        package_name = info[0]      
        package_votes = int(info[1])
        package_size = int(info[2])
 #this is just for seeing how the line was split up, remove later       
 #######print("Line split into a list: " + str(info)) #just showing the values that will be stored after getting split up from the read line
        ##Actually putting the values into the data fields for the classes in the list
        list[i].name = package_name    
        list[i].votes = package_votes
        list[i].size = package_size
       
  
    file.close() #close the input file, clean format
    ##just outputing what is in the class now for testing##############  this works so Subset function is good to go 
  #  for i in range(len(package_list)):
  #      print("Package Name: " + package_list[i].name)
  #      print("Package Size: " + str(package_list[i].size))  ##remember to cast type ints with str() when printing stuff
  #      print("Package Votes: " + str(package_list[i].votes))
  #      print("\n")
   
  #####We have our list, now need to do the functions to determine stuff############
    start = time.perf_counter()
    package_list = Subset(list)
    best = None

    for candidate in package_list:
      if Verify_Knapsack(candidate, w):
          if best is None or total_value(candidate) > total_value(best):
              best = candidate
    best_package_list = best
    end = time.perf_counter()

   ###OUTPUT RESULTS###################### 
    total_weight = 0
    for item in best_package_list:
       total_weight += item.size
    newFile = open("results.txt", 'w')
    for q in best_package_list:
        newFile.write(q.name)
        newFile.write(" ")
        newFile.write(str(q.size))
        newFile.write(" ")
        newFile.write(str(q.votes))
        newFile.write("\n")
    newFile.write("total size = " + str(total_weight) + " ")
    newFile.write("total votes = " + str(total_value(best_package_list)) + "\n")
    #display the time elasped
    newFile.write('elapsed time t = ' + str(end - start)) 

if __name__ == '__main__':
    main()
