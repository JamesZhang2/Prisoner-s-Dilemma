# This program is created by James Ruogu Zhang on August 18, 2019 and editted by James Ruogu Zhang later on
# This program is intended to simulate several strategies of Prisoner's Dilemma when there's probability of mistake
# The probability of mistake is p from [0,1]
# The computer generates a random number and compare it to p
# If the random number is greater than or equal to p, then the player betrays or cooperates as he wished
# If the random number is smaller than p, then the player betrays or cooperates when he wished to cooperate or betray, respectively
# Different strategies are considered, including Copycat, Copykitten, Copylion, etc.

import random

# p = float(input("Please enter the probability of mistake (p): "))
times = int(input("Please enter number of trials: "))


p1 = []
p2 = []
cc = []
cb = []
bc = []
bb = []

for i in range (0,100000):  # This is used repeatedly in every trial
    p1.append(0)
    p2.append(0)

for i in range (0,times):  # This records the number of cc, cb, bc, bb in each trial
    cc.append(0)
    cb.append(0)
    bc.append(0)
    bb.append(0)

# Copycat
print("Copycat:")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if (j == 0):
                p1[j] = 1
                p2[j] = 1
            else:
                p1[j] = p2[j-1]
                p2[j] = p1[j-1]
            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))

# Copykitten
print("------------------------------")
print("Copykitten:")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if (j == 0 or j == 1):
                p1[j] = 1
                p2[j] = 1
            else:
                if p2[j-2] == 0 and p2[j-1] == 0:
                    p1[j] = 0
                else:
                    p1[j] = 1

                if p1[j-2] == 0 and p1[j-1] == 0:
                    p2[j] = 0
                else:
                    p2[j] = 1

            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))

# Copylion
print("------------------------------")
print("Copylion:")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if j == 0:
                p1[j] = 1
                p2[j] = 1
            elif j == 1:
                p1[j] = p2[j-1]
                p2[j] = p1[j-1]
            else:
                if p2[j-2] == 1 and p2[j-1] == 1:
                    p1[j] = 1
                else:
                    p1[j] = 0

                if p1[j-2] == 1 and p1[j-1] == 1:
                    p2[j] = 1
                else:
                    p2[j] = 0

            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))

# Copy(or, 3)
print("------------------------------")
print("Copy(or, 3):")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if (j == 0 or j == 1 or j == 2):
                p1[j] = 1
                p2[j] = 1
            else:
                if p2[j-3] == 0 and p2[j-2] == 0 and p2[j-1] == 0:
                    p1[j] = 0
                else:
                    p1[j] = 1

                if p1[j-3] == 0 and p1[j-2] == 0 and p1[j-1] == 0:
                    p2[j] = 0
                else:
                    p2[j] = 1

            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))

# Copy(or, 10)
print("------------------------------")
print("Copy(or, 10):")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if (j <= 9):
                p1[j] = 1
                p2[j] = 1
            else:
                sum1 = 0
                sum2 = 0
                for l in range (1, 11):
                    sum1 += p2[j-l]
                    sum2 += p1[j-l]
                
                if sum1 == 0:
                    p1[j] = 0
                else:
                    p1[j] = 1

                if sum2 == 0:
                    p2[j] = 0
                else:
                    p2[j] = 1

            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))

# Copycat(p1) - Copykitten(p2)
print("------------------------------")
print("Copycat - Copykitten:")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if j == 0:
                p1[j] = 1
                p2[j] = 1
            elif j == 1:
                p1[j] = p2[j-1]
                p2[j] = 1
            else:
                p1[j] = p2[j-1]
                
                if p1[j-2] == 0 and p1[j-1] == 0:
                    p2[j] = 0
                else:
                    p2[j] = 1

            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))

# Copycat(p1) - Copy(or, 3)(p2)
print("------------------------------")
print("Copycat - Copy(or, 3):")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if j == 0:
                p1[j] = 1
                p2[j] = 1
            elif j == 1 or j == 2:
                p1[j] = p2[j-1]
                p2[j] = 1
            else:
                p1[j] = p2[j-1]
                
                if p1[j-3] + p1[j-2] + p1[j-1] == 0:
                    p2[j] = 0
                else:
                    p2[j] = 1

            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))

# Copycat(p1) - Random(r)(p2)
print("------------------------------")
r = float(input("Enter Probability of Cooperation for Random: "))
print("Copycat - Random(" + str(r) + "):")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if j == 0:
                p1[j] = 1
            else:
                p1[j] = p2[j-1]

            if random.random() < r:
                p2[j] = 1
            else:
                p2[j] = 0

            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))

# Copykitten(p1) - Copylion(p2)
print("------------------------------")
print("Copykitten - Copylion:")

for k in range (0,101):
    
    # initialize
    cctotal = 0
    cbtotal = 0
    bctotal = 0
    bbtotal = 0
    p = k/100
    for i in range (0,times):
        cc[i] = 0
        cb[i] = 0
        bc[i] = 0
        bb[i] = 0

    for i in range (0,times):
        for j in range (0,100000):
            if j == 0:
                p1[j] = 1
                p2[j] = 1
            elif j == 1:
                p1[j] = p2[j-1]
                p2[j] = p1[j-1]
            else:
                p1[j] = 1 - (1-p2[j-1]) * (1-p2[j-2])  # p1[j] = 0 iff p2[j-1] == 0 and p2[j-2] == 0
                p2[j] = p1[j-1] * p1[j-2]

            # Make mistakes
            if random.random() < p:
                p1[j] = 1-p1[j]

            if random.random() < p:
                p2[j] = 1-p2[j]
            # Result
            if j >= 50000:
                if p1[j] == 1 and p2[j] == 1:
                    cc[i] += 1

                if p1[j] == 1 and p2[j] == 0:
                    cb[i] += 1

                if p1[j] == 0 and p2[j] == 1:
                    bc[i] += 1

                if p1[j] == 0 and p2[j] == 0:
                    bb[i] += 1

        cctotal += cc[i]
        cbtotal += cb[i]
        bctotal += bc[i]
        bbtotal += bb[i]

    print("p =", p)
    print("cc =" , round(cctotal/times/50000,5))
    print("cb =" , round(cbtotal/times/50000,5))
    print("bc =" , round(bctotal/times/50000,5))
    print("bb =" , round(bbtotal/times/50000,5))
